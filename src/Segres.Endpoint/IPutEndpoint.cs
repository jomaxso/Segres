using Microsoft.AspNetCore.Http;
using Segres.Handlers;

namespace Segres.Endpoint;

public interface IPutEndpoint<in TRequest> : ICommandHandler<TRequest, IResult>
    where TRequest : IPutRequest
{
    Task<IResult> ICommandHandler<TRequest, IResult>.HandleAsync(TRequest command, CancellationToken cancellationToken)
        => ExecuteAsync(command, cancellationToken);
    
    public abstract Task<IResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}