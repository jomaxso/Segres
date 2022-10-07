using Segres.Contracts;

namespace Segres.UnitTest.Command;

public class ResultCommand : ICommand<bool>
{
    public int Number { get; init; }
}