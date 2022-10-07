using Segres.Contracts;

namespace Segres.UnitTest.Event.Objects;

public class DefaultMessage : IMessage
{
    public int Number { get; init; }
}