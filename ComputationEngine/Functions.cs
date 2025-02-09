using System.Collections.Concurrent;
using System.Diagnostics;

namespace ComputationEngine;

public static class Functions
{
    public static Result<string> FetchFromDatabase(ConcurrentDictionary<string, object> values)
    {
        var stopwatch = Stopwatch.StartNew();
        Thread.Sleep(2); // Simulating 1-2 ms work
        stopwatch.Stop();

        return new Result<string>("DB Result for Customer 123", "Database", stopwatch.Elapsed);
    }

    public static string FetchFromApi(ConcurrentDictionary<string, object> values)
    {
        return "API Response: Dummy Data";  // Returning raw string
    }

    public static Result<string> CombineResults(ConcurrentDictionary<string, object> values)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = $"Computed: {values["A"]} | {values["B"]}";
        stopwatch.Stop();

        return new Result<string>(result, "Computation Engine", stopwatch.Elapsed);
    }

    public static string ToUpperCase(ConcurrentDictionary<string, object> values)
    {
        return values["C"]?.ToString()?.ToUpper() ?? string.Empty;  // Raw string
    }

    public static object CheckCondition(ConcurrentDictionary<string, object> values)
    {
        return values["D"]?.ToString()?.Contains("TODO") ?? false;
    }

    public static Result<string> FinalFormat(ConcurrentDictionary<string, object> values)
    {
        var stopwatch = Stopwatch.StartNew();
        Thread.Sleep(2);
        var result = $"Final Result: {values["E"]}";
        stopwatch.Stop();

        return new Result<string>(result, "Formatter", stopwatch.Elapsed);
    }
}