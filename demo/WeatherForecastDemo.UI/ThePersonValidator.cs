using FluentValidation;
using Segres.Commons;
using Segres.Handlers;

namespace Segres;

public sealed class ThePersonHandler : IAsyncRequestHandler<ThePerson, Result<int>>
{
    public ValueTask<Result<int>> HandleAsync(ThePerson request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}