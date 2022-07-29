using MicrolisR;

namespace Demo.Endpoints.PrintToConsole;

public record PrintResult() : IValidatable
{
    public int Value { get; init; } = 0;
    public int Value2 { get; set; }
}