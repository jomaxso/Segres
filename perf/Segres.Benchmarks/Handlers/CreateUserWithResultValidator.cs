using FluentValidation;

namespace DispatchR.Benchmarks.Handlers;

public sealed class CreateUserWithResultValidator : AbstractValidator<CreateUserWithResult>
{
    public CreateUserWithResultValidator()
    {
        RuleFor(x => x.Number).GreaterThanOrEqualTo(0);
    }
}