namespace Segres.UnitTest.Requests;

public record DefaultIRequest : IRequest<bool>;

public class DefaultIRequestHandler : IRequestHandler<DefaultIRequest, bool>
{
    public async ValueTask<bool> HandleAsync(DefaultIRequest request, CancellationToken cancellationToken)
    {
        return await ValueTask.FromResult(true);
    }
}

public record DefaultRequest : IRequest<bool>;

public class DefaultRequestHandler : RequestHandler<DefaultRequest, bool>
{
    protected override bool Handle(DefaultRequest request)
    {
        return true;
    }
}

public record DefaultIRequestWithoutResponse : IRequest;

public class DefaultIRequestWithoutResponseHandler : IRequestHandler<DefaultIRequestWithoutResponse>
{

    public ValueTask HandleAsync(DefaultIRequestWithoutResponse request, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
}

public record DefaultRequestWithoutResponse : IRequest;

public class DefaultRequestWithoutResponseHandler : RequestHandler<DefaultRequestWithoutResponse>
{
    protected override void Handle(DefaultRequestWithoutResponse request)
    {
        
    }
}