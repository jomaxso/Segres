using Segres.Extensions.DependencyInjection.Microsoft;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Segres;
using Xunit;
using Xunit.Sdk;

namespace Segres.UnitTest.Command;

public class NoResultCommandHandlerTest
{
    private readonly IServiceProvider _serviceProvider = new ServiceCollection()
        .AddSegres(typeof(ResultCommand))
        .BuildServiceProvider();

    [Fact]
    public async Task CommandAsync_ShouldNotThrow_WhenCommandIsZeroCalled()
    {
        // Arrange
        var command = new NoResultCommand();
        var dispatcher = _serviceProvider.GetRequiredService<IMediator>();
        
        // Act
        var task = () => dispatcher.SendAsync(command);

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
        var dispatcher = _serviceProvider.GetRequiredService<IMediator>();
        
        // Act
        var result = async () => await dispatcher.SendAsync(command);

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
        var dispatcher = _serviceProvider.GetRequiredService<IMediator>();
        
        // Act
        var result = async () => await dispatcher.SendAsync(command);

        //Assert
        await result.Should().ThrowAsync<NotEmptyException>();
    }
}