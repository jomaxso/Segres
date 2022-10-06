using DispatchR.Extensions.DependencyInjection.Microsoft;
using DispatchR.UnitTest.Command;
using DispatchR.UnitTest.Event.Objects;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Sdk;

namespace DispatchR.UnitTest.Message;

public class DefaultMessageHandlerTest
{
    private readonly IServiceProvider _serviceProvider = new ServiceCollection()
        .AddDispatchR(typeof(ResultCommand))
        .BuildServiceProvider();

    [Fact]
    public async Task PublishAsync_ShouldNotThrow_WhenMessageIsZeroCalled()
    {
        // Arrange
        var message = new DefaultMessage();
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var task = async () => await dispatcher.PublishAsync(message);

        //Assert
        await task.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task PublishAsync_ShouldThrowIndexOutOfRangeException_WhenMessageIsGreaterThenZeroCalled()
    {
        // Arrange
        var message = new DefaultMessage()
        {
            Number = 100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var result = async () => await dispatcher.PublishAsync(message);

        //Assert
        await result.Should().ThrowAsync<IndexOutOfRangeException>();
    }
    
    [Fact]
    public async Task PublishAsync_ShouldThrowNotEmptyException_WhenMessageIsLessThenZeroCalled()
    {
        // Arrange
        var message = new DefaultMessage()
        {
            Number = -100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var result = async () => await dispatcher.PublishAsync(message);

        //Assert
        await result.Should().ThrowAsync<NotEmptyException>();
    }
    
    [Fact]
    public async Task PublishAsyncWhenAll_ShouldNotThrow_WhenMessageIsZeroCalled()
    {
        // Arrange
        var message = new DefaultMessage();
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var task = async () => await dispatcher.PublishAsync(message, Strategy.WhenAll);

        //Assert
        await task.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task PublishAsyncWhenAll_ShouldThrowIndexOutOfRangeException_WhenMessageIsGreaterThenZeroCalled()
    {
        // Arrange
        var message = new DefaultMessage()
        {
            Number = 100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var result = async () => await dispatcher.PublishAsync(message, Strategy.WhenAll);

        //Assert
        await result.Should().ThrowAsync<IndexOutOfRangeException>();
    }
    
    [Fact]
    public async Task PublishAsyncWhenAll_ShouldThrowNotEmptyException_WhenMessageIsLessThenZeroCalled()
    {
        // Arrange
        var message = new DefaultMessage()
        {
            Number = -100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var result = async () => await dispatcher.PublishAsync(message, Strategy.WhenAll);

        //Assert
        await result.Should().ThrowAsync<NotEmptyException>();
    }
    
    [Fact]
    public async Task PublishAsyncWhenAny_ShouldNotThrow_WhenMessageIsZeroCalled()
    {
        // Arrange
        var message = new DefaultMessage();
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var task = async () => await dispatcher.PublishAsync(message, Strategy.WhenAny);

        //Assert
        await task.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task PublishAsyncWhenAny_ShouldThrowIndexOutOfRangeException_WhenMessageIsGreaterThenZeroCalled()
    {
        // Arrange
        var message = new DefaultMessage()
        {
            Number = 100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var result = async () => await dispatcher.PublishAsync(message, Strategy.WhenAny);

        //Assert
        await result.Should().ThrowAsync<IndexOutOfRangeException>();
    }
    
    [Fact]
    public async Task PublishAsyncWhenAny_ShouldThrowNotEmptyException_WhenMessageIsLessThenZeroCalled()
    {
        // Arrange
        var message = new DefaultMessage()
        {
            Number = -100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var result = async () => await dispatcher.PublishAsync(message, Strategy.WhenAny);

        //Assert
        await result.Should().ThrowAsync<NotEmptyException>();
    }
    
    [Fact]
    public async Task PublishAsyncSequential_ShouldNotThrow_WhenMessageIsZeroCalled()
    {
        // Arrange
        var message = new DefaultMessage();
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var task = async () => await dispatcher.PublishAsync(message, Strategy.Sequential);

        //Assert
        await task.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task PublishAsyncSequential_ShouldThrowIndexOutOfRangeException_WhenMessageIsGreaterThenZeroCalled()
    {
        // Arrange
        var message = new DefaultMessage()
        {
            Number = 100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var result = async () => await dispatcher.PublishAsync(message, Strategy.Sequential);

        //Assert
        await result.Should().ThrowAsync<IndexOutOfRangeException>();
    }
    
    [Fact]
    public async Task PublishAsyncSequential_ShouldThrowNotEmptyException_WhenMessageIsLessThenZeroCalled()
    {
        // Arrange
        var message = new DefaultMessage()
        {
            Number = -100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var result = async () => await dispatcher.PublishAsync(message, Strategy.Sequential);

        //Assert
        await result.Should().ThrowAsync<NotEmptyException>();
    }
}