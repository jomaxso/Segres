using Demo.Domain.PrintToConsole;
using MicrolisR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Endpoints.PrintToConsole;

public class PrintRequest : IRequestable<PrintResult>, IMappable<PrintCommand>
{
    [FromRoute] 
    public int Value { get; set; }
}