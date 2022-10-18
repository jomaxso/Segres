using Microsoft.AspNetCore.Http;
using Segres.Handlers;

namespace Segres.Endpoint;

public interface IDeleteEndpoint<in TRequest> : ICommandHandler<TRequest, IResult>
    where TRequest : IDeleteRequest
{
    Task<IResult> ICommandHandler<TRequest, IResult>.HandleAsync(TRequest command, CancellationToken cancellationToken)
        => ExecuteAsync(command, cancellationToken);
    
    public abstract Task<IResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}