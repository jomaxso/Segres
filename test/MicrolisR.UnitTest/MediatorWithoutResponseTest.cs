using FluentAssertions;
using MicrolisR.Abstractions;
using MicrolisR.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MicrolisR.UnitTest;

public readonly record struct MediatorWithoutResponseRequest() : IRequest;
public readonly record struct MediatorWithoutResponseRequestWithoutHandler() : IRequest;

public class MediatorRequestWithoutResponseHandler : IReceiver<MediatorWithoutResponseRequest>
{
    public Task ReceiverAsync(MediatorWithoutResponseRequest request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class MediatorWithoutResponseTest
{
    [Fact]
    public async Task SendAsync_ShouldReturnTrue_WhenCalled()
    {
        // arrange 
        var serviceProvider = new ServiceCollection()
            .AddMicrolisR(typeof(MediatorWithResponseTest))     
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new MediatorWithoutResponseRequest();
        
        // act
        var result = () =>  mediator.SendAsync(request, CancellationToken.None);

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

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new MediatorWithoutResponseRequest();

        // act
        var result = () => mediator.SendAsync(request, CancellationToken.None);

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

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new MediatorWithoutResponseRequestWithoutHandler();

        // act
        var result = () => mediator.SendAsync(request, CancellationToken.None);

        // assert
        await result.Should().ThrowAsync<Exception>();
    }
}