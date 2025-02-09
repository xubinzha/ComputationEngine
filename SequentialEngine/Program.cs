// See https://aka.ms/new-console-template for more information

var engine = new SequentialEngine.SequentialEngine();

// Load dependencies and function mappings
engine.LoadDependencies(Path.Combine(Environment.CurrentDirectory, "dependencies.csv"));
engine.LoadFunctionMappings(Path.Combine(Environment.CurrentDirectory, "functions.csv"));

// Execute the computation graph
engine.Execute();
engine.PrintValues();