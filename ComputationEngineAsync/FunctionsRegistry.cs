using System.Collections.Concurrent;

namespace ComputationEngineAsync;

public static class FunctionsRegistry
{
    public static readonly Dictionary<string, Func<ConcurrentDictionary<string, object>, Task<object>>> FunctionMap =
        new()
        {
            { "FetchFromDatabase", Functions.FetchFromDatabase },
            { "FetchFromApi", Functions.FetchFromApi },
            { "CombineResults", Functions.CombineResults },
            { "ToUpperCase", Functions.ToUpperCase },
            { "CheckCondition", Functions.CheckCondition },
            { "FinalFormat", Functions.FinalFormat }
        };
}