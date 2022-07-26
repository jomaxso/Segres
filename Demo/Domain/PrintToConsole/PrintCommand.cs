using Demo.Endpoints.PrintToConsole;
using MicrolisR;


namespace PrintToConsole;

public record PrintCommand() : 
    IRequestable<bool>,
    IMappable<PrintResult>
{
    public int Value { get; init; } = 0;
    public int Value2 { get; init; } = 0;
}