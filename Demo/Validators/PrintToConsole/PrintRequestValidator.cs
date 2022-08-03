using Demo.Domain.PrintToConsole;
using Demo.Endpoints.PrintToConsole;
using MicrolisR;

namespace Demo.Validators.PrintToConsole;

public class PrintRequestValidator : IValidationHandler<PrintRequest>
{
    public void Validate(PrintRequest value)
    {
        if (value.Value is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(PrintCommand.Value),"Must be between 0 and 100");

        Console.WriteLine("Valid [PrintRequest]");
    }
}