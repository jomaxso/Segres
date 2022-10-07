using Segres.Contracts;

namespace Segres.UnitTest.Event.Objects;

public class TwoHandlerMessage : IMessage
{
    public int Number { get; init; }
}