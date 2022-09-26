using System.Reflection;
using DispatchR.Contracts;
using DispatchR.Extensions.DependencyInjection.Microsoft;
using DispatchR.UnitTest.Dispatcher;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DispatchR.UnitTest.Sender;

public readonly record struct RequestWithoutResponse(int Value) : ICommand;
public readonly record struct RequestWithoutHandlerAndResponse() : ICommand;

public class WithoutResponseHandler : ICommandHandler<RequestWithoutResponse>
{
    public Task HandleAsync(RequestWithoutResponse request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class SenderWithoutResponseTest
{
    [Fact]
    public async Task SendAsync_ShouldReturnTrue_WhenCalled()
    {
        // arrange 
        var serviceProvider = new ServiceCollection()
            .AddDispatchR(Assembly.GetExecutingAssembly())
            .BuildServiceProvider();

        var sender = serviceProvider.GetRequiredService<ISender>();
        var request = new RequestWithoutResponse();
        
        // act
        var result = () =>  sender.SendAsync(request, CancellationToken.None);

        // assert
        await result.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task SendAsync_ShouldReturnValueOfRequest_WhenCalled()
    {
        // arrange 
        var serviceProvider = new ServiceCollection()
            .AddDispatchR(typeof(MediatorWithResponseTest))
            .BuildServiceProvider();

        var sender = serviceProvider.GetRequiredService<ISender>();
        var request = new RequestWithoutResponse(4712);

        // act
        var result = () =>  sender.SendAsync(request, CancellationToken.None);

        // assert
        await result.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task SendAsync_ShouldThrow_WhenNoHandlerFound()
    {
        // arrange 
        var serviceProvider = new ServiceCollection()
            .AddDispatchR(typeof(MediatorWithResponseTest))
            .BuildServiceProvider();

        var sender = serviceProvider.GetRequiredService<ISender>();
        var request = new RequestWithoutHandlerAndResponse();

        // act
        var result = () => sender.SendAsync(request, CancellationToken.None);

        // assert
        await result.Should().ThrowAsync<Exception>();
    }
}