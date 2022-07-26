using MicrolisR;

namespace PrintToConsole;

public class PrintRequest : 
    IRequestable, 
    IMappable<PrintCommand>
{
    public int Value { get; set; }
    public int Value2 { get; set; }
}