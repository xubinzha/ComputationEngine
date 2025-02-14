using System.Collections.Concurrent;
using System.Diagnostics;
using SingleThreadedEngine;

namespace PerformanceTests;

public static class PerformanceDemoTests
{
    public static void PerformanceTest(int nodes = 1000, int maxEdges = 5)
    {
        var sequentialEngine = new SequentialEngine.SequentialEngine();
        var singleThreadEngine = new SingleThreadEngine();
        var computationEngine = new ComputationEngine.ComputationEngine(10);
        Random random = new();
        
        // Generate Graph
        var dependencyGraph = new List<List<string>>();
        for (var i = 0; i < nodes; i++)
        {
            var fromNode = "Node_" + i;
            var edges = random.Next(0, maxEdges);
            dependencyGraph.Add([fromNode, fromNode]);
            
            if (i < nodes - 1)
            {
                for (var j = 0; j < edges; j++)
                {
                    var toNode = "Node_" + random.Next(i + 1, nodes); // Ensure acyclic structure
                    dependencyGraph.Add([fromNode, toNode]);
                }
            }
        }
        
        // Generate Functions
        var computeFunctions = new Dictionary<string, Func<Dictionary<string, object>, object>>();

        for (var i = 0; i < nodes; i++)
        {
            var index = i;
            var nodeName = "Node_" + index;
            computeFunctions[nodeName] = values =>
            {
                // Simulate heavy computation
                Thread.Sleep(2); // Simulate workload
                return Math.Sqrt(index) + (values.ContainsKey("Node_" + (index - 1)) ? (double) values["Node_" + (index - 1)] : 0);
            };
        }
        
        var computeFunctionsWithConcurrentInput = new Dictionary<string, Func<ConcurrentDictionary<string, object>, object>>();

        for (var i = 0; i < nodes; i++)
        {
            var index = i;
            var nodeName = "Node_" + index;
            computeFunctionsWithConcurrentInput[nodeName] = values =>
            {
                // Simulate heavy computation
                Thread.Sleep(2); // Simulate workload
                return Math.Sqrt(index) + (values.ContainsKey("Node_" + (index - 1)) ? (double) values["Node_" + (index - 1)] : 0);
            };
        }
        
        // **Measure Sequential Execution Time**
        var sw = Stopwatch.StartNew();
        sw.Restart();
        sequentialEngine.LoadDependencies(dependencyGraph);
        sequentialEngine.LoadFunctionMappings(computeFunctions);
        sequentialEngine.Execute();
        sw.Stop();
        Console.WriteLine($"🔴 Sequential Execution Time: {sw.ElapsedMilliseconds} ms, with {nodes} nodes");
        
        // **Measure Single-Threaded Execution Time**
        sw.Restart();
        singleThreadEngine.LoadDependencies(dependencyGraph);
        singleThreadEngine.LoadFunctionMappings(computeFunctions);
        singleThreadEngine.Execute();
        sw.Stop();
        Console.WriteLine($"🔴 Single-Threaded Execution Time: {sw.ElapsedMilliseconds} ms, with {nodes} nodes");

        // **Measure Multi-Threaded Execution Time**
        sw.Restart();
        computationEngine.LoadDependencies(dependencyGraph);
        computationEngine.LoadFunctionMappings(computeFunctionsWithConcurrentInput);
        computationEngine.Execute();
        sw.Stop();
        Console.WriteLine($"🟢 Multi-Threaded Execution Time: {sw.ElapsedMilliseconds} ms, with {nodes} nodes");
    }
}