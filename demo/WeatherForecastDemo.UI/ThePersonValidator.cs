using FluentValidation;

namespace Segres;

public sealed class ThePersonHandler : IRequestHandler<ThePerson, Result<int>>
{
    public ValueTask<Result<int>> HandleAsync(ThePerson request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}