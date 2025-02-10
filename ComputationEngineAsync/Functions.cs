using System.Collections.Concurrent;
using System.Diagnostics;

namespace ComputationEngineAsync;

public static class Functions
{
    public static async Task<object> FetchFromDatabase(ConcurrentDictionary<string, object> values)
    {
        var stopwatch = Stopwatch.StartNew();
        await Task.Delay(500);
        stopwatch.Stop();
        
        return new Result<string>("DB Result for Customer 123", "Database", stopwatch.Elapsed);
    }

    public static async Task<object> FetchFromApi(ConcurrentDictionary<string, object> values)
    {
        await Task.Delay(500);
        return "API Response: Dummy Data";  // Returning raw string
    }

    public static async Task<object> CombineResults(ConcurrentDictionary<string, object> values)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = $"Computed: {values["A"]} | {values["B"]}";
        await Task.Delay(500);
        stopwatch.Stop();

        return new Result<string>(result, "Computation Engine", stopwatch.Elapsed);
    }

    public static async Task<object> ToUpperCase(ConcurrentDictionary<string, object> values)
    {
        await Task.Delay(500);
        return values["C"]?.ToString()?.ToUpper() ?? string.Empty;  // Raw string
    }

    public static async Task<object> CheckCondition(ConcurrentDictionary<string, object> values)
    {
        await Task.Delay(500);
        return values["D"]?.ToString()?.Contains("TODO") ?? false;
    }

    public static async Task<object> FinalFormat(ConcurrentDictionary<string, object> values)
    {
        var stopwatch = Stopwatch.StartNew();
        await Task.Delay(500);
        var result = $"Final Result: {values["E"]}";
        stopwatch.Stop();

        return new Result<string>(result, "Formatter", stopwatch.Elapsed);
    }
}