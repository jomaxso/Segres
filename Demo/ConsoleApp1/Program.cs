// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using ConsoleApp1;
using MicrolisR;
using MicrolisR.Pipelines;
using WebApplication1.Endpoints;
using WebApplication1.Endpoints.WeatherForecast;
using WebApplication1.Models;

BenchmarkRunner.Run<Benchy>();
// Console.WriteLine();
//
// IMediator mediator = new Mediator(typeof(Program));
//
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