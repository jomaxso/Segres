using DispatchR.Contracts;

namespace DispatchR.UnitTest.Event.Objects;

public class DefaultMessage : IMessage
{
    public int Number { get; init; }
}