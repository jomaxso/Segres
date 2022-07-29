using Demo.Domain.PrintToConsole;
using MicrolisR;

namespace Demo.Validators.PrintToConsole;

public class PrintCommandValidator : IValidationHandler<PrintCommand>
{
    public void Validate(PrintCommand value)
    {
        if (value.Value is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(PrintCommand.Value),"Must be between 0 and 100");
    }
}