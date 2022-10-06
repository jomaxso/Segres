using DispatchR.Extensions;
using DispatchR.Extensions.DependencyInjection.Microsoft;
using DispatchR.UnitTest.Stream.Objects;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DispatchR.UnitTest.Stream;

public class StreamExtensionsTest
{
    private readonly IServiceProvider _serviceProvider = new ServiceCollection()
        .AddDispatchR(typeof(StreamExtensionsTest))
        .BuildServiceProvider();

    
    [Fact]
    public async Task StreamExtentionWhere_ShouldReturnIntegersGreater5_WhenIterated()
    {
        // Arrange
        var streamOption = new IntegerStream();
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();

        var last = 6;

        // Act
        var stream = dispatcher.CreateStreamAsync(streamOption).Where(x => x > 5);

        //Assert
        await foreach (var item in stream)
        {
            item.Should().Be(last);
            last++;
        }
    }
    
    [Fact]
    public async Task StreamExtentionSelect_ShouldReturnIntegersAsString_WhenIterated()
    {
        // Arrange
        var streamOption = new IntegerStream();
        var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();

        var last = 0;

        // Act
        var stream = dispatcher.CreateStreamAsync(streamOption).Select(x => x.ToString());

        //Assert
        await foreach (var item in stream)
        {
            item.Should().Be(last.ToString());
            last++;
        }
    }
}