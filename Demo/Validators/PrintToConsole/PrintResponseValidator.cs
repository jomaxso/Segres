using MicrolisR;
using PrintToConsole;

namespace Demo.Validators;

public class PrintResponseValidator : IValidationHandler<PrintResult>
{
    public void Validate(PrintResult value)
    {
        if (value.Value is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(PrintCommand.Value),"Must be between 0 and 100");
    }
}