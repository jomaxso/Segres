using FluentAssertions;
using MicrolisR.Extensions.Microsoft.DependencyInjection;
using MicrolisR.UnitTest.Mediator;
using MicrolisR.UnitTest.Sender;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MicrolisR.UnitTest.Publisher;



public class PublisherWithoutResponseTest
{
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