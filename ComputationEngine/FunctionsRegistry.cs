using System.Collections.Concurrent;

namespace ComputationEngine;

public static class FunctionsRegistry
{
    public static readonly Dictionary<string, Func<ConcurrentDictionary<string, object>, object>> FunctionMap =
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