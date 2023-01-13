using Microsoft.AspNetCore.Http;
using Segres;
using Segres.AspNetCore;

public record Test() : IHttpRequest<bool>
{
    public static string RequestRoute => "";

    public static RequestType RequestType => RequestType.Get;
}

public sealed class TestEndpoint : AbstractEndpoint<Test, bool>
{
    public override async ValueTask<HttpResult<bool>> ResolveAsync(Test request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return HttpResult<bool>.Create(Results.Ok(true));
    }
}