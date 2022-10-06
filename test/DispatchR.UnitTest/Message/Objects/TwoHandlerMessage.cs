using DispatchR.Contracts;

namespace DispatchR.UnitTest.Event.Objects;

public class TwoHandlerMessage : IMessage
{
    public int Number { get; init; }
}