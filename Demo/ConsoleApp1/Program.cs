// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using ConsoleApp1;
using MicrolisR;
using MicrolisR.Pipelines;
using WebApplication1.Endpoints;
using WebApplication1.Endpoints.WeatherForecast;
using WebApplication1.Models;

var result = await Benchy.SendAsync(new WeatherForecastGetByIdRequest());

Console.WriteLine();

BenchmarkRunner.Run<Benchy>();
// return;
// Console.WriteLine();
//
// var v = new WeatherForecastGetByIdRequest();
// IMediator mediator = new Mediator(typeof(Program));
// await mediator.SendAsync(v);
// //

// var rr = new WeatherForecastGetByIdRequest();
// var rType = rr.GetType();
// var handlerType = typeof(GetByIdEndpoint);
//
// var result = await SendAsync(rr, CancellationToken.None);
//
// Console.WriteLine();
//
//
// Task<TResponse> SendAsync<TResponse>(IQueryRequest<TResponse> request, CancellationToken cancellationToken = default)
// {
//     var requestType = request.GetType();
//
//     var handlerType = typeof(GetByIdEndpoint); //_requestHandlerDetails[requestType];
//     
//     var handlerAsObject = new GetByIdEndpoint() as object
//         ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");
//
//     var handlerDelegate = (object h, object r, CancellationToken c) => ((GetByIdEndpoint) h).HandleAsync(r as WeatherForecastGetByIdRequest, c);
//     
//     return (handlerDelegate.Invoke(handlerAsObject, request, cancellationToken) as Task<TResponse>)!;
// }


//
//
// public delegate Task<TResponse> PipelineDelegate<TResponse>(PipelineContext<IQueryRequest<TResponse>, TResponse> context);
//
// public readonly record struct PipelineContext<TRequest, TResponse>(TRequest Request, CancellationToken CancellationToken) where TRequest : IQueryRequest<TResponse>;
//
//
// public interface IPipelineBehaior<TRequest, TResponse>
//     where TRequest : IQueryRequest<TResponse>
// {
//     Task<TResponse> InvokeAsync(TRequest request, CancellationToken cancellationToken);
// }
//
//
// public class PipelineBehaior : IPipelineBehaior<WeatherForecastGetByIdRequest, WeatherForecast>
// {
//
//     private readonly PipelineDelegate<WeatherForecast> next;
//
//     public PipelineBehaior(PipelineDelegate<WeatherForecast> next)
//     {
//         this.next = next;
//     }
//
//     public async Task<WeatherForecast> InvokeAsync(WeatherForecastGetByIdRequest request, CancellationToken cancellationToken)
//     {
//         var id = request.Id;
//         request.Id = id + 1;
//         // before
//         Console.WriteLine("before" + id);
//         var response = await next(request, cancellationToken);
//         // after 
//         Console.WriteLine("after" + id);
//
//         return response;
//     }
// }