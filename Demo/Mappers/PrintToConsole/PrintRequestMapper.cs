using Demo.Domain.PrintToConsole;
using Demo.Endpoints.PrintToConsole;
using MicrolisR;
using Utilities;

namespace Demo.Mappers.PrintToConsole;

public class PrintRequestMapper : IMapHandler<PrintRequest, PrintCommand>
{
    public PrintCommand Map(PrintRequest request)
    {
        return new PrintCommand()
        {
            Value = request.Value,
        };
    }
}