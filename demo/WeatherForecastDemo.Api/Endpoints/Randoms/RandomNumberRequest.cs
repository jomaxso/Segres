using System.Runtime.CompilerServices;
using Segres;
using Segres.AspNetCore;

namespace WeatherForecastDemo.Api.Endpoints.Randoms;

public sealed class RandomNumbersEndpoint : AbstractEndpoint<RandomNumbersRequest, IAsyncEnumerable<int>>
{
    private readonly RandomService _randomService;

    public RandomNumbersEndpoint(RandomService randomService)
    {
        _randomService = randomService;
    }

    public override void Configure(RouteHandlerBuilder routeBuilder)
    {
        routeBuilder.WithTags("Random");
    }

    public override async ValueTask<HttpResult<IAsyncEnumerable<int>>> ResolveAsync(RandomNumbersRequest request, CancellationToken cancellationToken)
    {
        var enumerable = CreateNumbersAsync(request.Length);
        await ValueTask.CompletedTask;
        return Ok(enumerable);
    }

    private async IAsyncEnumerable<int> CreateNumbersAsync(int length)
    {
        var unique = new HashSet<int>();

        for (var i = 0; i < length; i++)
        {
            await ValueTask.CompletedTask;
            var n = _randomService.GetNext(1, length);
            
            while (!unique.Add(n))
            {
                n = _randomService.GetNext(1, length);
            }
            
            yield return n;
        }
    }
}

public sealed class RandomNumbersRequest : IHttpRequest<IAsyncEnumerable<int>>
{
    public static string RequestRoute => "RandomNumbers";

    public static RequestType RequestType => RequestType.Get;

    public int Length { get; init; }
}