using DispatchR.Contracts;

namespace DispatchR.UnitTest.Command;

public class NoResultCommand : ICommand
{
    public int Number { get; init; }
}

