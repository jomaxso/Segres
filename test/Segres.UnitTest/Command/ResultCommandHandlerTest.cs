using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Segres.Abstractions;
using Xunit;
using Xunit.Sdk;

namespace Segres.UnitTest.Command;

public class Dispatcher_ResultCommandHandlerTest
{
    private readonly IServiceProvider _serviceProvider = TestServiceCollection.CreateServiceProvider();

    [Fact]
    public async Task CommandAsync_ShouldReturnTrue_WhenCommandIsZeroCalled()
    {
        // Arrange
        var command = new ResultCommand();
        var dispatcher = _serviceProvider.GetRequiredService<ISender>();


        // Act
        var result = await dispatcher.SendAsync(command);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CommandAsync_ShouldThrowIndexOutOfRangeException_WhenCommandIsGreaterThenZeroCalled()
    {
        // Arrange
        var command = new ResultCommand
        {
            Number = 100
        };
        var dispatcher = _serviceProvider.GetRequiredService<ISender>();

        // Act
        var result = async () => await dispatcher.SendAsync(command);

        //Assert
        await result.Should().ThrowAsync<IndexOutOfRangeException>();
    }

    [Fact]
    public async Task CommandAsync_ShouldThrowNotEmptyException_WhenCommandIsLessThenZeroCalled()
    {
        // Arrange
        var command = new ResultCommand
        {
            Number = -100
        };
        var dispatcher = _serviceProvider.GetRequiredService<ISender>();

        // Act
        var result = async () => await dispatcher.SendAsync(command);

        //Assert
        await result.Should().ThrowAsync<NotEmptyException>();
    }
}