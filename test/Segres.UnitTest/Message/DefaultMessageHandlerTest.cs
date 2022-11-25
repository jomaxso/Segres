using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Segres.UnitTest.Event.Objects;
using Xunit;
using Xunit.Sdk;

namespace Segres.UnitTest.Message;

public class DefaultMessageHandlerTest
{
    private readonly IServiceProvider _serviceProvider = TestServiceCollection.CreateServiceProvider();

    [Fact]
    public async Task PublishAsync_ShouldNotThrow_WhenMessageIsZeroCalled()
    {
        // Arrange
        var message = new DefaultNotification();
        var dispatcher = _serviceProvider.GetRequiredService<IPublisher>();

        // Act
        var task = async () => await dispatcher.PublishAsync(message);

        //Assert
        await task.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PublishAsync_ShouldThrowIndexOutOfRangeException_WhenMessageIsGreaterThenZeroCalled()
    {
        // Arrange
        var message = new DefaultNotification
        {
            Number = 100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IPublisher>();

        // Act
        var result = async () => await dispatcher.PublishAsync(message);

        //Assert
        await result.Should().ThrowAsync<IndexOutOfRangeException>();
    }

    [Fact]
    public async Task PublishAsync_ShouldThrowNotEmptyException_WhenMessageIsLessThenZeroCalled()
    {
        // Arrange
        var message = new DefaultNotification
        {
            Number = -100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IPublisher>();

        // Act
        var result = async () => await dispatcher.PublishAsync(message);

        //Assert
        await result.Should().ThrowAsync<NotEmptyException>();
    }

    [Fact]
    public async Task PublishAsyncWhenAll_ShouldNotThrow_WhenMessageIsZeroCalled()
    {
        // Arrange
        var message = new DefaultNotification();
        var dispatcher = _serviceProvider.GetRequiredService<IPublisher>();

        // Act
        var task = async () => await dispatcher.PublishAsync(message, PublishStrategy.WhenAll);

        //Assert
        await task.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PublishAsyncWhenAll_ShouldThrowIndexOutOfRangeException_WhenMessageIsGreaterThenZeroCalled()
    {
        // Arrange
        var message = new DefaultNotification
        {
            Number = 100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IPublisher>();

        // Act
        var result = async () => await dispatcher.PublishAsync(message, PublishStrategy.WhenAll);

        //Assert
        await result.Should().ThrowAsync<IndexOutOfRangeException>();
    }

    [Fact]
    public async Task PublishAsyncWhenAll_ShouldThrowNotEmptyException_WhenMessageIsLessThenZeroCalled()
    {
        // Arrange
        var message = new DefaultNotification
        {
            Number = -100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IPublisher>();

        // Act
        var result = async () => await dispatcher.PublishAsync(message, PublishStrategy.WhenAll);

        //Assert
        await result.Should().ThrowAsync<NotEmptyException>();
    }

    [Fact]
    public async Task PublishAsyncWhenAny_ShouldNotThrow_WhenMessageIsZeroCalled()
    {
        // Arrange
        var message = new DefaultNotification();
        var dispatcher = _serviceProvider.GetRequiredService<IPublisher>();

        // Act
        var task = async () => await dispatcher.PublishAsync(message, PublishStrategy.WhenAny);

        //Assert
        await task.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PublishAsyncWhenAny_ShouldThrowIndexOutOfRangeException_WhenMessageIsGreaterThenZeroCalled()
    {
        // Arrange
        var message = new DefaultNotification
        {
            Number = 100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IPublisher>();

        // Act
        var result = async () => await dispatcher.PublishAsync(message, PublishStrategy.WhenAny);

        //Assert
        await result.Should().ThrowAsync<IndexOutOfRangeException>();
    }

    [Fact]
    public async Task PublishAsyncWhenAny_ShouldThrowNotEmptyException_WhenMessageIsLessThenZeroCalled()
    {
        // Arrange
        var message = new DefaultNotification
        {
            Number = -100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IPublisher>();

        // Act
        var result = async () => await dispatcher.PublishAsync(message, PublishStrategy.WhenAny);

        //Assert
        await result.Should().ThrowAsync<NotEmptyException>();
    }

    [Fact]
    public async Task PublishAsyncSequential_ShouldNotThrow_WhenMessageIsZeroCalled()
    {
        // Arrange
        var message = new DefaultNotification();
        var dispatcher = _serviceProvider.GetRequiredService<IPublisher>();

        // Act
        var task = async () => await dispatcher.PublishAsync(message, PublishStrategy.Sequential);

        //Assert
        await task.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PublishAsyncSequential_ShouldThrowIndexOutOfRangeException_WhenMessageIsGreaterThenZeroCalled()
    {
        // Arrange
        var message = new DefaultNotification
        {
            Number = 100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IPublisher>();

        // Act
        var result = async () => await dispatcher.PublishAsync(message, PublishStrategy.Sequential);

        //Assert
        await result.Should().ThrowAsync<IndexOutOfRangeException>();
    }

    [Fact]
    public async Task PublishAsyncSequential_ShouldThrowNotEmptyException_WhenMessageIsLessThenZeroCalled()
    {
        // Arrange
        var message = new DefaultNotification
        {
            Number = -100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IPublisher>();

        // Act
        var result = async () => await dispatcher.PublishAsync(message, PublishStrategy.Sequential);

        //Assert
        await result.Should().ThrowAsync<NotEmptyException>();
    }
}