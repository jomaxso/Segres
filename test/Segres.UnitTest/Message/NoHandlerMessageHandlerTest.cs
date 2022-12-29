using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Segres.Abstractions;
using Segres.UnitTest.Event.Objects;
using Xunit;

namespace Segres.UnitTest.Message;

public class NoHandlerMessageHandlerTest
{
    private readonly IServiceProvider _serviceProvider = TestServiceCollection.CreateServiceProvider();

    [Fact]
    public async Task PublishAsync_ShouldNotThrow_WhenMessageIsZeroCalled()
    {
        // Arrange
        var message = new NoHandlerNotification();
        var dispatcher = _serviceProvider.GetRequiredService<IPublisher>();

        // Act
        var task = async () => await dispatcher.PublishAsync(message);

        //Assert
        await task.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PublishAsyncWhenAll_ShouldNotThrow_WhenMessageIsZeroCalled()
    {
        // Arrange
        var message = new NoHandlerNotification();
        var dispatcher = _serviceProvider.GetRequiredService<IPublisher>();

        // Act
        var task = async () => await dispatcher.PublishAsync(message);

        //Assert
        await task.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PublishAsyncWhenAny_ShouldNotThrow_WhenMessageIsZeroCalled()
    {
        // Arrange
        var message = new NoHandlerNotification();
        var dispatcher = _serviceProvider.GetRequiredService<IPublisher>();

        // Act
        var task = async () => await dispatcher.PublishAsync(message);

        //Assert
        await task.Should().NotThrowAsync();
    }
}