using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using BenchmarkDotNet.Attributes;
using MicrolisR;
using MicrolisR.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Endpoints;
using WebApplication1.Endpoints.WeatherForecast;
using WebApplication1.Models;

namespace ConsoleApp1;

[MemoryDiagnoser(false)]
public class Benchy
{
    private static readonly ServiceProvider ServiceProvider = new ServiceCollection()
        .AddMicrolisR(typeof(Program))
        .AddSingleton<ValidationBehavior>()
        .BuildServiceProvider();

    private static readonly IMediator Mediator = ServiceProvider.GetRequiredService<IMediator>();
    
    private static readonly WeatherForecastGetByIdRequest Request2 = new();
    private static readonly WeatherForecastGetByIdRequest Request3 = new();

    [Benchmark]
    public async Task<WeatherForecast?> SendAsyncReflection() => await SendAsync(Request2, CancellationToken.None);
    
    [Benchmark]
    public async Task<WeatherForecast> SendAsync() => await Mediator.SendAsync(Request3, CancellationToken.None);

    private static readonly Dictionary<Type, (Type Type, Delegate Del)> dic = new()
    {
        {typeof(WeatherForecastGetByIdRequest), (typeof(GetByIdEndpoint), DynamicHandler.CreateQueryDelegate<WeatherForecast>(typeof(WeatherForecastGetByIdRequest)))}
    };

    public static Task<TResponse> SendAsync<TResponse>(IQueryRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        if (!dic.ContainsKey(requestType))
            throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerType = dic[requestType];

        var handlerAsObject = ServiceProvider.GetService(handlerType.Type)
                              ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        return handlerType.Del.InvokeQueryHandler(handlerAsObject, request, cancellationToken)!;
    }
    


    public void CreateHandler(object handler, Type handlerType)
    {

        var assemblyName = new AssemblyName("InternalHandlerCreaterAssembly");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name!);
        var typeBuilder = moduleBuilder.DefineType("InternalHandlerCaster", TypeAttributes.NotPublic);
        var methodBuilder = typeBuilder.DefineMethod("Cast", MethodAttributes.Public | MethodAttributes.Static, handlerType, new[] {typeof(object)});
        
        var gen = methodBuilder.GetILGenerator();
        
        
        
        
        #region VERSION 1

        // IL_0000: nop
        
        // IL_0001: newobj instance void Test.GetByIdEndpoint::.ctor()
        // IL_0006: stloc.0
        
        // IL_0007: newobj instance void Test.WeatherForecastGetByIdRequest::.ctor()
        // IL_000c: stloc.1
        
        // IL_000d: ldloc.0
        // IL_000e: castclass Test.GetByIdEndpoint
        // IL_0013: stloc.2
        
        // IL_0014: ldloc.1
        // IL_0015: castclass Test.WeatherForecastGetByIdRequest
        // IL_001a: stloc.3
        
        // IL_001b: ret

        #endregion    

        #region VERSION 2

        // IL_0000: nop
        
        // IL_0001: ldarg.1
        // IL_0002: castclass Test.GetByIdEndpoint
        // IL_0007: stloc.0
        
        // IL_0008: ldarg.2
        // IL_0009: castclass Test.WeatherForecastGetByIdRequest
        // IL_000e: stloc.1
        
        // IL_000f: ret

        #endregion
        
        
    }


}

