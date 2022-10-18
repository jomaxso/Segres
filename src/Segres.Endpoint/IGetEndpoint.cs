using Microsoft.AspNetCore.Http;
using Segres.Handlers;

namespace Segres.Endpoint;

public interface IGetEndpoint<in TRequest> : IQueryHandler<TRequest, IResult>
    where TRequest : IGetRequest
{
    Task<IResult> IQueryHandler<TRequest, IResult>.HandleAsync(TRequest query, CancellationToken cancellationToken)
        => ExecuteAsync(query, cancellationToken);
    
    public abstract Task<IResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}