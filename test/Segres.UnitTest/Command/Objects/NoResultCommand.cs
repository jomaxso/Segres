using Segres.Contracts;

namespace Segres.UnitTest.Command;

public class NoResultCommand : IRequest
{
    public int Number { get; init; }
}