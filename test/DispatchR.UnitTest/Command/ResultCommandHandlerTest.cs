using DispatchR.Extensions.DependencyInjection.Microsoft;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Sdk;

namespace DispatchR.UnitTest.Command;

public class Dispatcher_ResultCommandHandlerTest
{
    private readonly IServiceProvider _serviceProvider = new ServiceCollection()
        .AddDispatchR(typeof(ResultCommand))
        .BuildServiceProvider();

    [Fact]
    public async Task CommandAsync_ShouldReturnTrue_WhenCommandIsZeroCalled()
    {
        // Arrange
        var command = new ResultCommand();
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        
        // Act
        var result = await dispatcher.CommandAsync(command);

        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task CommandAsync_ShouldThrowIndexOutOfRangeException_WhenCommandIsGreaterThenZeroCalled()
    {
        // Arrange
        var command = new ResultCommand()
        {
            Number = 100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var result = async () => await dispatcher.CommandAsync(command);

        //Assert
        await result.Should().ThrowAsync<IndexOutOfRangeException>();
    }
    
    [Fact]
    public async Task CommandAsync_ShouldThrowNotEmptyException_WhenCommandIsLessThenZeroCalled()
    {
        // Arrange
        var command = new ResultCommand()
        {
            Number = -100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var result = async () => await dispatcher.CommandAsync(command);

        //Assert
        await result.Should().ThrowAsync<NotEmptyException>();
    }
}