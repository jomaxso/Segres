using FluentAssertions;
using MicrolisR.Abstractions;
using MicrolisR.Extensions.Microsoft.DependencyInjection;
using MicrolisR.UnitTest.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MicrolisR.UnitTest.Sender;

public readonly record struct RequestWithoutResponse(int Value) : IRequest;
public readonly record struct RequestWithoutHandlerAndResponse() : IRequest;

public class RequestWithoutResponseHandler : IReceiver<RequestWithoutResponse>
{
    public Task ReceiveAsync(RequestWithoutResponse request, CancellationToken cancellationToken)
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
            .AddMicrolisR(typeof(MediatorWithResponseTest))
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
            .AddMicrolisR(typeof(MediatorWithResponseTest))
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
            .AddMicrolisR(typeof(MediatorWithResponseTest))
            .BuildServiceProvider();

        var sender = serviceProvider.GetRequiredService<ISender>();
        var request = new RequestWithoutHandlerAndResponse();

        // act
        var result = () => sender.SendAsync(request, CancellationToken.None);

        // assert
        await result.Should().ThrowAsync<Exception>();
    }
}