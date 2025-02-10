// See https://aka.ms/new-console-template for more information

var engine = new ComputationEngineAsync.ComputationEngineAsync(4);

// Load dependencies and function mappings
engine.LoadDependencies("dependencies.csv");
engine.LoadFunctionMappings("functions.csv");

// Execute the computation graph asynchronously
engine.Execute();
engine.PrintValues();