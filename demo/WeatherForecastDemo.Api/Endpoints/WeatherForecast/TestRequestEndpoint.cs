using Segres;
using Segres.Abstractions;
using Segres.AspNetCore;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

// [HttpPostRequest(group: "Test")]
public record struct TestRequest : IRequest<Guid>;

public class TestRequestEndpoint : IAsyncRequestEndpoint<TestRequest, Guid>
{
    private readonly ISender _sender;
    private readonly ConsoleLogger _logger;

    public TestRequestEndpoint(ISender sender, ConsoleLogger logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public async ValueTask<Guid> HandleAsync(TestRequest request, CancellationToken cancellationToken)
    {
        var stream = await _sender.SendAsync(new MyStream(), cancellationToken);

        var guid = "";
        
        await foreach (var c in stream.WithCancellation(cancellationToken))
        {
            guid += c;
        }

        _logger.Log(guid);
        
        return Guid.Parse(guid);
    }

    public static void Configure(IEndpointDefinition endpoint)
    {
        endpoint
            .WithGroup("Tests")
            .MapPost("Test")
            .Produces<Guid>(200, "application/json")
            .ProducesProblem(400, "application/json");
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