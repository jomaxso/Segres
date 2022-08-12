using MicrolisR.Validation;

namespace Demo;

internal class TestValidator : IValidation<RequestMain>
{
    public void Validate(RequestMain requestMain)
    {
        var x = requestMain.RuleFor(x => x.Percentage)
            .IsExclusiveBetween(0, 100);
        // Console.WriteLine(nameof(Validator));
    }
}