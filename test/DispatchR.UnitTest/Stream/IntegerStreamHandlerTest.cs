using DispatchR.Extensions.DependencyInjection.Microsoft;
using DispatchR.UnitTest.Stream.Objects;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DispatchR.UnitTest.Stream;

public class IntegerStreamHandlerTest
{
    private readonly IServiceProvider _serviceProvider = new ServiceCollection()
        .AddDispatchR(typeof(IntegerStream))
        .BuildServiceProvider();

    
    [Fact]
    public async Task CreateStreamAsync_ShouldReturnIntegersFrom0To10_WhenIterated()
    {
        // Arrange
        var streamOption = new IntegerStream();
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();

        var last = 0;

        // Act
        var stream = dispatcher.CreateStreamAsync(streamOption);

        //Assert
        await foreach (var item in stream)
        {
            item.Should().Be(last);
            last++;
        }
    }
    
    [Fact]
    public async Task StreamAsyncWithCancellation_ShouldReturnIntegersFrom0To10_WhenIterated()
    {
        // Arrange
        var streamOption = new IntegerStream();
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();

        var last = 0;

        // Act
        await dispatcher.StreamAsync(streamOption, Callback, CancellationToken.None);

        //Assert
        ValueTask Callback(int item, CancellationToken ct)
        {
            item.Should().Be(last);
            last++;
            return ValueTask.CompletedTask;
        }
    }
    
    [Fact]
    public async Task StreamAsyncWithoutCancellation_ShouldReturnIntegersFrom0To10_WhenIterated()
    {
        // Arrange
        var streamOption = new IntegerStream();
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();

        var last = 0;

        // Act
        await dispatcher.StreamAsync(streamOption, Callback, CancellationToken.None);

        //Assert
        ValueTask Callback(int item)
        {
            item.Should().Be(last);
            last++;
            return ValueTask.CompletedTask;
        }
    }

   
}