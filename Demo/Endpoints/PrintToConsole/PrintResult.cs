using MicrolisR;

namespace PrintToConsole;

public record PrintResult() : IMappable<bool>
{
    public int Value { get; init; } = 0;
    public int Value2 { get; set; }
}