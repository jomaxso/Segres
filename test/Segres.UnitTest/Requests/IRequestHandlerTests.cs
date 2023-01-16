using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Segres.UnitTest.Requests;

public class IRequestHandlerTests
{
    private readonly IMediator _mediator;

    public IRequestHandlerTests()
    {
        _mediator = TestServiceCollection.CreateServiceProvider().GetRequiredService<IMediator>();
    }

    [Fact]
    public async ValueTask SendAsync_ShouldReturnTrue_WhenCalled()
    {
        // Arrange
        var request = new DefaultIRequest();

        // Act
        var result = await _mediator.SendAsync(request);

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void SendAsync_ShouldNotThrow_WhenCalled()
    {
        // Arrange
        var request = new DefaultIRequestWithoutResponse();

        // Act
        var resultTask = () => _mediator.SendAsync(request);

        // Assert
        resultTask.Should().NotThrow();
    }

    [Fact]
    public void Send_ShouldReturnTrue_WhenCalled()
    {
        // Arrange
        var request = new DefaultIRequest();

        // Act
        var result = _mediator.Send(request);

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void Send_ShouldNotThrow_WhenCalled()
    {
        // Arrange
        var request = new DefaultIRequestWithoutResponse();

        // Act
        var resultTask = () => _mediator.Send(request);

        // Assert
        resultTask.Should().NotThrow();
    }
}