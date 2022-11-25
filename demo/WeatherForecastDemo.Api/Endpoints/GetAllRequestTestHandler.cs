using Segres.Tmp.Http;


namespace WeatherForecastDemo.Api.Endpoints;
public record GetAllRequestTest : IHttpRequest<IEnumerable<int>>;

public class GetAllRequestTestHandler : IEndpoint<GetAllRequestTest, IEnumerable<int>>
{
    public async ValueTask<IEnumerable<int>> ExecuteAsync(GetAllRequestTest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return Enumerable.Range(0, 100).Select(x => x);
    }
}

[HttpGet("tests","{from:int}/{till:int}")]
public record GetRangedRequestTest(int From, int Till) : IHttpRequest<IEnumerable<int>>;

public class GetRangedRequestTestHandler : IEndpoint<GetRangedRequestTest, IEnumerable<int>>
{
    public async ValueTask<IEnumerable<int>> ExecuteAsync(GetRangedRequestTest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return Enumerable.Range(0, 100).Select(x => x).Where(x => x >= request.From && x <= request.Till);
    }
}