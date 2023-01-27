using System.Text.Json;
using Segres.AspNetCore;
using WeatherForecastDemo.Api.Services;

namespace WeatherForecastDemo.Api.Endpoints.Randoms;

public sealed class RandomNumberRequest : IHttpRequest<int>
{
    public static string RequestRoute => "Random/{Max}";
    public static RequestType RequestType => RequestType.Get;

    public int? Min { get; init; }
    public int Max { get; init; }
}

public sealed class RandomNumberEndpoint : AbstractEndpoint<RandomNumberRequest, int>
{
    private readonly RandomService _randomService;

    public RandomNumberEndpoint(RandomService randomService)
    {
        _randomService = randomService;
    }

    public override async ValueTask<HttpResult<int>> ResolveAsync(RandomNumberRequest request, CancellationToken cancellationToken)
    {
        await ValueTask.CompletedTask;
        Console.WriteLine(JsonSerializer.Serialize(request));
        return _randomService.GetNext(request.Min ?? 0, request.Max);
    }

    public override void Configure(RouteHandlerBuilder routeBuilder)
    {
        routeBuilder
            .WithTags("Random");
    }
}