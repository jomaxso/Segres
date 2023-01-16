using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Segres.UnitTest.Requests;

public class RequestHandlerTests
{
    private readonly IMediator _mediator;

    public RequestHandlerTests()
    {
        _mediator = TestServiceCollection.CreateServiceProvider().GetRequiredService<IMediator>();
    }

    [Fact]
    public async ValueTask SendAsync_ShouldReturnTrue_WhenCalled()
    {
        // Arrange
        var request = new DefaultRequest();

        // Act
        var result = await _mediator.SendAsync(request);

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void SendAsync_ShouldNotThrow_WhenCalled()
    {
        // Arrange
        var request = new DefaultRequestWithoutResponse();

        // Act
        var resultTask = () => _mediator.SendAsync(request);

        // Assert
        resultTask.Should().NotThrow();
    }

    [Fact]
    public void Send_ShouldReturnTrue_WhenCalled()
    {
        // Arrange
        var request = new DefaultRequest();

        // Act
        var result = _mediator.Send(request);

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void Send_ShouldNotThrow_WhenCalled()
    {
        // Arrange
        var request = new DefaultRequestWithoutResponse();

        // Act
        var resultTask = () => _mediator.Send(request);

        // Assert
        resultTask.Should().NotThrow();
    }
}