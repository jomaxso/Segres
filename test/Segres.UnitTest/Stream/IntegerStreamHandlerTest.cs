using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Segres.UnitTest.Stream.Objects;
using Xunit;

namespace Segres.UnitTest.Stream;

public class IntegerStreamHandlerTest
{
    private readonly IServiceProvider _serviceProvider = TestServiceCollection.CreateServiceProvider();

    [Fact]
    public async Task CreateStreamAsync_ShouldReturnIntegersFrom0To10_WhenIterated()
    {
        // Arrange
        var streamOption = new IntegerStreamRequest();
        var dispatcher = _serviceProvider.GetRequiredService<ISender>();

        var last = 0;

        // Act
        var stream = await dispatcher.SendAsync(streamOption);

        //Assert
        await foreach (var item in stream)
        {
            item.Should().Be(last);
            last++;
        }
    }
}