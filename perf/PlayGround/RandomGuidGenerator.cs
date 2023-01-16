namespace PlayGround;

public interface ISomeService
{
    public void PrintSomething();
}

public class SomeServiceOne : ISomeService
{
    private readonly IRandomGuidGenerator _randomGuidGenerator;

    public SomeServiceOne(IRandomGuidGenerator randomGuidGenerator)
    {
        _randomGuidGenerator = randomGuidGenerator;
    }

    public void PrintSomething() => Console.WriteLine(_randomGuidGenerator.RandomGuid);
}

public interface IRandomGuidGenerator
{
    Guid RandomGuid { get; set; }
}

public sealed class RandomGuidGenerator : IRandomGuidGenerator
{
    public Guid RandomGuid { get; set; } = Guid.NewGuid();
}