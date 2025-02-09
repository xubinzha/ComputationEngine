namespace SequentialEngine;

public class SequentialEngine()
{
    private readonly List<string> _nodes = [];
    private readonly Dictionary<string, Func<Dictionary<string, object>, object>> _functionMap = new();
    private readonly Dictionary<string, object> _calculatedValues = new();
    
    public void LoadDependencies(List<List<string>> computationGraph)
    {
        _nodes.Clear();

        foreach (var edge in computationGraph)
        {
            _nodes.AddRange([edge[0], edge[1]]);
        }
    }
    
    public void LoadDependencies(string filePath)
    {
        _nodes.Clear();
        
        foreach (var line in File.ReadLines(filePath).Skip(1))
        {
            var parts = line.Split(',');
            var from = parts[0].Trim();
            var to = parts[1].Trim();
            _nodes.AddRange([from, to]);
        }
    }
    
    public void LoadFunctionMappings(Dictionary<string, Func<Dictionary<string, object>, object>> functionMap)
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
        foreach (var node in _nodes.OrderBy(x => x))
        {
            _calculatedValues[node] = _functionMap[node](_calculatedValues);
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