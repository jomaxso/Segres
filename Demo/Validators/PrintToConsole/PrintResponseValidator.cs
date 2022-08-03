using Demo.Domain.PrintToConsole;
using Demo.Endpoints.PrintToConsole;
using MicrolisR;

namespace Demo.Validators.PrintToConsole;

public class PrintResponseValidator : IValidationHandler<PrintResult>
{
    public void Validate(PrintResult value)
    {
        if (value.Value is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(PrintCommand.Value),"Must be between 0 and 100");
        
        Console.WriteLine("Valid [PrintResult]");
    }
}