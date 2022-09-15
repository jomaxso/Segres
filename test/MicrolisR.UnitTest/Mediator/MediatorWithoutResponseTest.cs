using FluentAssertions;
using MicrolisR.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MicrolisR.UnitTest.Mediator;

public readonly record struct MediatorWithoutResponseRequest() : ICommandRequest;
public readonly record struct MediatorWithoutResponseRequestWithoutHandler() : ICommandRequest;

public class MediatorRequestWithoutResponseHandler : ICommandRequestHandler<MediatorWithoutResponseRequest>
{
    public Task HandleAsync(MediatorWithoutResponseRequest request, CancellationToken cancellationToken)
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
    
    [Fact]
    public async Task PublishAsync_ShouldReturnTrue_WhenCalled()
    {
        // arrange 
        var serviceProvider = new ServiceCollection()
            .AddMicrolisR(typeof(MediatorWithResponseTest))
            .BuildServiceProvider();

        var publisher = serviceProvider.GetRequiredService<IPublisher>();
        var request = new NotificationWithoutResponse();
        
        // act
        var result = () =>  publisher.PublishAsync(request, CancellationToken.None);

        // assert
        await result.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task PublishAsync_ShouldReturnValueOfRequest_WhenCalled()
    {
        // arrange 
        var serviceProvider = new ServiceCollection()
            .AddMicrolisR(typeof(MediatorWithResponseTest))
            .BuildServiceProvider();

        var publisher = serviceProvider.GetRequiredService<IPublisher>();
        var request = new NotificationWithoutResponse(4712);

        // act
        var result = () =>  publisher.PublishAsync(request, CancellationToken.None);

        // assert
        await result.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task PublishAsync_ShouldThrow_WhenNoHandlerFound()
    {
        // arrange 
        var serviceProvider = new ServiceCollection()
            .AddMicrolisR(typeof(MediatorWithResponseTest))
            .BuildServiceProvider();

        var publisher = serviceProvider.GetRequiredService<IPublisher>();
        var request = new NotificationWithoutHandlerAndResponse();

        // act
        var result = () => publisher.PublishAsync(request, CancellationToken.None);

        // assert
        await result.Should().ThrowAsync<Exception>();
    }
}