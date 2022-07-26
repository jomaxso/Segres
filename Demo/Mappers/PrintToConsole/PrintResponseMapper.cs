using Demo.Endpoints.PrintToConsole;
using MicrolisR;
using PrintToConsole;

namespace Demo.Mappers.PrintToConsole;

public class PrintResponseMapper : IMapHandler<PrintResult, bool>
{
    public bool Map(PrintResult request)
    {
        return true;
    }
}