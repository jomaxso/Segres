using Segres.Contracts;

namespace Segres.UnitTest.Query;

public class ResultQuery : IQuery<string>
{
    public int Number { get; init; }
}