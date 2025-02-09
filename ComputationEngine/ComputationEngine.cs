using System.Collections.Concurrent;

namespace ComputationEngine;

public class ComputationEngine(int maxDegreeOfParallelism = 4)
{
    private readonly Dictionary<string, List<string>> _computationGraph = new();
    private readonly ConcurrentDictionary<string, int> _inDegree = new();
    private readonly ConcurrentDictionary<string, object> _calculatedValues = new();
    private readonly ConcurrentQueue<string> _taskQueue = new();
    private readonly ManualResetEventSlim _completionEvent = new();
    private int _runningTasks = 0;
    private readonly SemaphoreSlim _semaphore = new(maxDegreeOfParallelism);
    private readonly Dictionary<string, Func<ConcurrentDictionary<string, object>, object>> _functionMap = new();

    private void AddEdge(string from, string to)
    {
        _computationGraph.TryAdd(from, []);
        _computationGraph.TryAdd(to, []);
        _inDegree.TryAdd(from, 0);

        if (from != to)
        {
            _computationGraph[from].Add(to);
            _inDegree[to] = _inDegree.GetValueOrDefault(to, 0) + 1;
        }
    }
    
    public void LoadDependencies(Dictionary<string, string> computationGraph)
    {
        _computationGraph.Clear();

        foreach (var pair in computationGraph)
        {
            AddEdge(pair.Key, pair.Value);
            AddEdge(pair.Key, pair.Key);
        }
    }
    
    public void LoadDependencies(string filePath)
    {
        _computationGraph.Clear();
        
        foreach (var line in File.ReadLines(filePath).Skip(1))
        {
            var parts = line.Split(',');
            var from = parts[0].Trim();
            var to = parts[1].Trim();
            AddEdge(from, to);
            AddEdge(from, from);
        }
    }
    
    public void LoadFunctionMappings(Dictionary<string, Func<ConcurrentDictionary<string, object>, object>> functionMap)
    {
        _functionMap.Clear();
        
        foreach (var pair in functionMap)
        {
            _functionMap.TryAdd(pair.Key, pair.Value);
        }
    }
    
    public void LoadFunctionMappings(string filePath)
    {
        _functionMap.Clear();
        
        foreach (var line in File.ReadLines(filePath).Skip(1))
        {
            var parts = line.Split(',');
            var node = parts[0].Trim();
            var functionName = parts[1].Trim();

            if (FunctionsRegistry.FunctionMap.TryGetValue(functionName, out var function))
            {
                _functionMap[node] = function;
            }
            else
            {
                throw new Exception($"Function {functionName} not found");
            }
        }
    }
    
    public void Execute()
    {
        foreach (var node in _inDegree.Where(kv => kv.Value == 0).Select(kv => kv.Key))
        {
            _taskQueue.Enqueue(node);
        }

        _runningTasks = _taskQueue.Count;
        if (_runningTasks == 0)
        {
            _completionEvent.Set();
            return;
        }

        while (!_taskQueue.IsEmpty)
        {
            if (_taskQueue.TryDequeue(out var node))
            {
                ThreadPool.QueueUserWorkItem(_ => ComputeNode(node));
            }
        }

        // Wait until all computations are done
        _completionEvent.Wait();
    }

    private void ComputeNode(string node)
    {
        _semaphore.Wait();
        
        try
        {
            // Compute the node's value
            if (_functionMap.TryGetValue(node, out var function))
            {
                _calculatedValues.TryAdd(node, function(_calculatedValues));
            }

            if (_computationGraph.TryGetValue(node, out var neighbors))
            {
                foreach (var neighbor in neighbors.Where(neighbor => _inDegree.AddOrUpdate(neighbor, 0, (_, count) => count - 1) == 0))
                {
                    _taskQueue.Enqueue(neighbor);
                    Interlocked.Increment(ref _runningTasks);
                    ThreadPool.QueueUserWorkItem(_ => ComputeNode(neighbor));
                }
            }
        }
        finally
        {
            _semaphore.Release();
            if (Interlocked.Decrement(ref _runningTasks) == 0)
            {
                _completionEvent.Set();
            }
        }
    }

    public void PrintValues()
    {
        foreach (var kvp in _calculatedValues)
        {
            Console.WriteLine($"{kvp.Key} ({kvp.Value?.GetType().Name}): {kvp.Value}");
        }
    }
}