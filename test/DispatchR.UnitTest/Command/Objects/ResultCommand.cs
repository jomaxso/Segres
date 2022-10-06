using DispatchR.Contracts;

namespace DispatchR.UnitTest.Command;

public class ResultCommand : ICommand<bool>
{
    public int Number { get; init; }
}