using Segres.Contracts;

namespace Segres.UnitTest.Command;

public class NoResultCommand : ICommand
{
    public int Number { get; init; }
}

