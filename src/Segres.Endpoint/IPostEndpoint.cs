using Microsoft.AspNetCore.Http;
using Segres.Handlers;

namespace Segres.Endpoint;

public interface IPostEndpoint<in TRequest> : ICommandHandler<TRequest, IResult>
    where TRequest : IPostRequest
{
    Task<IResult> ICommandHandler<TRequest, IResult>.HandleAsync(TRequest command, CancellationToken cancellationToken)
        => ExecuteAsync(command, cancellationToken);
    public abstract Task<IResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}