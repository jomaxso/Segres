using DispatchR.Extensions.DependencyInjection.Microsoft;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Sdk;

namespace DispatchR.UnitTest.Query;

public class ResultQueryHandlerTest
{
    private readonly IServiceProvider _serviceProvider = new ServiceCollection()
        .AddDispatchR(typeof(ResultQuery))
        .BuildServiceProvider();

    [Fact]
    public async Task QueryAsync_ShouldReturnZero_WhenNumberIsZero()
    {
        // Arrange
        var command = new ResultQuery();
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();

        // Act
        var numberAsString = await dispatcher.QueryAsync(command);

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
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var result = async () => await dispatcher.QueryAsync(query);

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
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var result = async () => await dispatcher.QueryAsync(query);

        //Assert
        await result.Should().ThrowAsync<NotEmptyException>();
    }
}