using Demo.Domain.PrintToConsole;
using Demo.Endpoints.PrintToConsole;
using MicrolisR;

namespace Demo.Mappers.PrintToConsole;

public class PrintCommandMapper : IMapHandler<PrintCommand, PrintResult>
{
    public PrintResult Map(PrintCommand request)
    {
        return new PrintResult()
        {
            Value = request.Value,
            Value2 = request.Value2
        };
    }
}