namespace DispatchR.Benchmarks.Handlers;

public class BenchmarkService
{
    // public static readonly int[] ListOfNumbers = Enumerable.Range(1, 1).Select(x => x).ToArray();
    public static readonly int?[] ListOfNumbers = {null};
    private const int number = 0;

    public async ValueTask<int> RunAsync()
    {
        await Task.Delay(100);
        return number;
    }
}