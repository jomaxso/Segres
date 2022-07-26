using MicrolisR;
using PrintToConsole;
using Utilities;

namespace Demo.Mappers.PrintToConsole;

public class PrintRequestMapper : IMapHandler<PrintRequest, PrintCommand>
{
    public PrintCommand Map(PrintRequest request)
    {
        return new PrintCommand()
        {
            Value = request.Value,
            Value2 = request.Value2
        };
    }
}