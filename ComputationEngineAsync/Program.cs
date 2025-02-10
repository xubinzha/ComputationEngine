// See https://aka.ms/new-console-template for more information

var engine = new ComputationEngineAsync.ComputationEngineAsync(2);

// Load dependencies and function mappings
engine.LoadDependencies("dependencies.csv");
engine.LoadFunctionMappings("functions.csv");

// Execute the computation graph asynchronously
await engine.Execute();
engine.PrintValues();