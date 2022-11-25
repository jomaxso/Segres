using System.Net.Http.Json;
using System.Reflection;

namespace Segres.Tmp.Http;

public interface IHttpRequest<TResponse>
{
}

public readonly record struct HttpRequest<TRequest, TResult>(TRequest Request)
    where TRequest : IHttpRequest<TResult>
{
    public EndpointAttribute Definition { get; } = typeof(TRequest).GetCustomAttributes().OfType<EndpointAttribute>().First() ?? throw new Exception();
}

public sealed class HttpRequestHandler<TRequest, TResult>
    where TRequest : IHttpRequest<TResult?>
{
    // private readonly IHttpClientFactory _clientFactory;
    //
    //
    // public HttpRequestHandler(IHttpClientFactory clientFactory)
    // {
    //     _clientFactory = clientFactory;
    // }
    //
    public ValueTask<TResult?> HandleAsync(HttpRequest<TRequest, TResult?> request, CancellationToken cancellationToken = default)
    {
        // var client = _clientFactory.CreateClient(request.Definition.GroupName);

        // return request.Definition.HttpMethod switch
        // {
        //     Http.POST =>  PostAsync(client, request, cancellationToken),
        //     Http.GET =>  GetAsync(client, request, cancellationToken),
        //     Http.PUT =>  PutAsync(client, request, cancellationToken),
        //     Http.DELETE =>  DeleteAsync(client, request, cancellationToken),
        //     _ => throw new ArgumentOutOfRangeException()
        // };

        return ValueTask.FromResult<TResult?>(default);
    }

    private static async ValueTask<TResult?> PostAsync(HttpClient client, HttpRequest<TRequest, TResult?> httpRequest, CancellationToken cancellationToken)
    {
        var response = await client.PostAsJsonAsync(httpRequest.Definition.GroupName + "/" + httpRequest.Definition.RoutePattern, httpRequest.Request, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken);
    }
    
    private static async ValueTask<TResult?> GetAsync(HttpClient client, HttpRequest<TRequest, TResult?> httpRequest, CancellationToken cancellationToken)
    {
        return await client.GetFromJsonAsync<TResult>(httpRequest.Definition.GroupName + "/" + httpRequest.Definition.RoutePattern, cancellationToken);
    }
    
    private static async ValueTask<TResult?> PutAsync(HttpClient client, HttpRequest<TRequest, TResult?> httpRequest, CancellationToken cancellationToken)
    {
        var response =  await client.PutAsJsonAsync(httpRequest.Definition.GroupName + "/" + httpRequest.Definition.RoutePattern, httpRequest.Request, cancellationToken: cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken);
    }

    private static async ValueTask<TResult?> DeleteAsync(HttpClient client, HttpRequest<TRequest, TResult?> httpRequest, CancellationToken cancellationToken)
    {
        return await client.GetFromJsonAsync<TResult>(httpRequest.Definition.GroupName + "/" + httpRequest.Definition.RoutePattern, cancellationToken);
    }
}

public sealed class HttpPostRequestHandler<TRequest, TResult>
    where TRequest : IHttpRequest<TResult?>
{
    private readonly HttpClient _client;
    
    public HttpPostRequestHandler(HttpClient client)
    {
        _client = client;
    }
    
    public async ValueTask<TResult?> HandleAsync(HttpRequest<TRequest, TResult?> request, CancellationToken cancellationToken = default)
    {
        if (request.Definition.HttpMethod != Http.POST) 
            throw new Exception();
        
        var response = await _client.PostAsJsonAsync(request.Definition.GroupName + "/" + request.Definition.RoutePattern, request.Request, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken);

    }
}