using MicrolisR.Validation;

namespace Demo;

internal class TestValidator : IValidation<RequestMain>
{
    public void Validate(RequestMain requestMain)
    {
        // requestMain.RuleFor().IsNotNull();
        //
        // requestMain.RuleFor(x => x.Percentage)
        //     .IsBetween(0, 100);
        // Console.WriteLine(nameof(Validator));
    }
}