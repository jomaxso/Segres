using Segres.Contracts;

namespace Segres.UnitTest.Query;

public class ResultQuery : IRequest<string>
{
    public int Number { get; init; }
}