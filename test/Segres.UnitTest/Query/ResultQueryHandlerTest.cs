using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Segres.Extensions.DependencyInjection.Microsoft;
using Xunit;
using Xunit.Sdk;

namespace Segres.UnitTest.Query;

public class ResultQueryHandlerTest
{
    private readonly IServiceProvider _serviceProvider = new ServiceCollection()
        .AddSegres(typeof(ResultQuery))
        .BuildServiceProvider();

    [Fact]
    public async Task QueryAsync_ShouldReturnZero_WhenNumberIsZero()
    {
        // Arrange
        var command = new ResultQuery();
        var dispatcher = _serviceProvider.GetRequiredService<IServiceBroker>();

        // Act
        var numberAsString = await dispatcher.SendAsync(command);

        //Assert
        numberAsString.Should().Be("Zero");
    }
    
    [Fact]
    public async Task QueryAsync_ShouldThrowIndexOutOfRangeException_WhenCommandIsGreaterThenZeroCalled()
    {
        // Arrange
        var query = new ResultQuery()
        {
            Number = 100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IServiceBroker>();
        
        // Act
        var result = async () => await dispatcher.SendAsync(query);

        //Assert
        await result.Should().ThrowAsync<IndexOutOfRangeException>();
    }
    
    [Fact]
    public async Task QueryAsync_ShouldThrowNotEmptyException_WhenCommandIsLessThenZeroCalled()
    {
        // Arrange
        var query = new ResultQuery()
        {
            Number = -100
        };
        var dispatcher = _serviceProvider.GetRequiredService<IServiceBroker>();
        
        // Act
        var result = async () => await dispatcher.SendAsync(query);

        //Assert
        await result.Should().ThrowAsync<NotEmptyException>();
    }
}