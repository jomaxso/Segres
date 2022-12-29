using Segres.Abstractions;

namespace Segres.UnitTest.Command;

public class NoResultCommand : IRequest
{
    public int Number { get; init; }
}