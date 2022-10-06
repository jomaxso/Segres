using DispatchR.Contracts;

namespace DispatchR.UnitTest.Query;

public class ResultQuery : IQuery<string>
{
    public int Number { get; init; }
}