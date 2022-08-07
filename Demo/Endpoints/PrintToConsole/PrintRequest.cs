using MicrolisR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Endpoints.PrintToConsole;

public class PrintRequest : IRequestable<bool>
{
    [FromRoute] 
    public int Value { get; set; }
}