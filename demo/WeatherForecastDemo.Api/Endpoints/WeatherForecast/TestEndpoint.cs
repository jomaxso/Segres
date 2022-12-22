using Segres;
using Segres.AspNet;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

[HttpPostRequest(group: "Test")]
public record struct TestRequest : IHttpRequest;






public class TestEndpoint : AbstractEndpoint<TestRequest>
{
    public override async ValueTask<IHttpResult> HandleAsync(TestRequest request, CancellationToken cancellationToken)
    {
        await ValueTask.CompletedTask;
        return Ok();
    }
}

