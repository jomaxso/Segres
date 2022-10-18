using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Segres.Extensions.DependencyInjection.Microsoft;
using Segres.UnitTest.Command;
using Segres.UnitTest.Event.Objects;
using Xunit;

namespace Segres.UnitTest.Message;

public class NoHandlerMessageHandlerTest
{
    private readonly IServiceProvider _serviceProvider = new ServiceCollection()
        .AddSegres(typeof(ResultCommand))
        .BuildServiceProvider();

    [Fact]
    public async Task PublishAsync_ShouldNotThrow_WhenMessageIsZeroCalled()
    {
        // Arrange
        var message = new NoHandlerMessage();
        var dispatcher = _serviceProvider.GetRequiredService<IServiceBroker>();
        
        // Act
        var task = async () => await dispatcher.PublishAsync(message);

        //Assert
        await task.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PublishAsyncWhenAll_ShouldNotThrow_WhenMessageIsZeroCalled()
    {
        // Arrange
        var message = new NoHandlerMessage();
        var dispatcher = _serviceProvider.GetRequiredService<IServiceBroker>();
        
        // Act
        var task = async () => await dispatcher.PublishAsync(message, Strategy.WhenAll);

        //Assert
        await task.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PublishAsyncWhenAny_ShouldNotThrow_WhenMessageIsZeroCalled()
    {
        // Arrange
        var message = new NoHandlerMessage();
        var dispatcher = _serviceProvider.GetRequiredService<IServiceBroker>();
        
        // Act
        var task = async () => await dispatcher.PublishAsync(message, Strategy.WhenAny);

        //Assert
        await task.Should().NotThrowAsync();
    }
}