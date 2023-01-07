using Microsoft.AspNetCore.Mvc;
using Segres.Abstractions;
using Segres.AspNetCore;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;


public record struct TestRequest([FromRoute]int Id, [FromBody]string? Name) : IRequest<Guid>;

[HttpGetRequest("Endpoints/Tests/Test2")]
public record struct TestRequest2 : IRequest<Guid>;

[HttpGetRequest("Endpoints/Test2")]
public record struct TestRequest3 : IRequest<Guid>;

public class TestRequestEndpoint : AbstractEndpoint<TestRequest, Guid>
{
    private readonly ISender _sender;
    private readonly IConsoleLogger _logger;

    public TestRequestEndpoint(ISender sender, IConsoleLogger logger)
    {
        _sender = sender;
        _logger = logger;
    }

    protected override async ValueTask<IEndpointResult<Guid>> ResolveAsync(TestRequest request, CancellationToken cancellationToken)
    {
        var stream = await _sender.SendAsync(new MyStream(), cancellationToken);

        var guid = "";
        
        await foreach (var c in stream.WithCancellation(cancellationToken))
        {
            guid += c;
        }

        _logger.Log(guid);

        var result = Guid.Parse(guid);
        return EndpointResult.Ok(result);
    }
}




public record MyStream() : IRequest<IAsyncEnumerable<char>>;

public class MyStreamHandler : IRequestHandler<MyStream, IAsyncEnumerable<char>>
{
    public async IAsyncEnumerable<char> Handle(MyStream request)
    {
        var guid = Guid.NewGuid().ToString();
        
        foreach (var t in guid)
        {
            yield return await ValueTask.FromResult(t);
        }
    }
}