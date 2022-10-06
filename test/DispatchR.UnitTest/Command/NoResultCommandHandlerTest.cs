using DispatchR.Extensions.DependencyInjection.Microsoft;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Sdk;

namespace DispatchR.UnitTest.Command;

public class NoResultCommandHandlerTest
{
    private readonly IServiceProvider _serviceProvider = new ServiceCollection()
        .AddDispatchR(typeof(ResultCommand))
        .BuildServiceProvider();

    [Fact]
    public async Task CommandAsync_ShouldNotThrow_WhenCommandIsZeroCalled()
    {
        // Arrange
        var command = new NoResultCommand();
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var task = () => dispatcher.CommandAsync(command);

        //Assert
        await task.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task CommandAsync_ShouldThrowIndexOutOfRangeException_WhenCommandIsGreaterThenZeroCalled()
    {
        // Arrange
        var command = new NoResultCommand()
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
        var command = new NoResultCommand()
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