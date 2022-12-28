using Segres.Contracts;

namespace Segres.UnitTest.Command;

public class ResultCommand : IRequest<bool>
{
    public int Number { get; init; }
}