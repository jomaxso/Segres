using Segres;

namespace DispatchR.Benchmarks.Handlers;

public class BenchmarkService
{
    private const int number = 0;

    // public static readonly int[] ListOfNumbers = Enumerable.Range(1, 1).Select(x => x).ToArray();
    public static readonly int?[] ListOfNumbers = {null};

    public async ValueTask<int> RunAsync()
    {
        await ValueTask.CompletedTask;
        return number;
    }
    
    public int Run()
    {
        return number;
    }
}


