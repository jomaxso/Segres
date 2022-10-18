using Segres.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Segres.Extensions.DependencyInjection.Microsoft;
using Segres.UnitTest.Stream.Objects;
using Xunit;

namespace Segres.UnitTest.Stream;

public class StreamExtensionsTest
{
    private readonly IServiceProvider _serviceProvider = new ServiceCollection()
        .AddSegres(typeof(StreamExtensionsTest))
        .BuildServiceProvider();

    
    [Fact]
    public async Task StreamExtentionWhere_ShouldReturnIntegersGreater5_WhenIterated()
    {
        // Arrange
        var streamOption = new IntegerStream();
        var dispatcher = _serviceProvider.GetRequiredService<IServiceBroker>();

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
        var dispatcher = _serviceProvider.GetRequiredService<IServiceBroker>();

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