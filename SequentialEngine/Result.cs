namespace SequentialEngine;

public class Result<T>(T value, string source, TimeSpan computationTime)
{
    public T Value { get; } = value;
    public string Source { get; } = source;
    public TimeSpan ComputationTime { get; } = computationTime;

    public override string ToString()
    {
        return $"[{Source}] Value: {Value}, Time: {ComputationTime.TotalMilliseconds} ms";
    }
}