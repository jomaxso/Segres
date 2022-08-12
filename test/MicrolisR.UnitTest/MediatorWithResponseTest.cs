using FluentAssertions;
using MicrolisR.Abstractions;
using MicrolisR.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MicrolisR.UnitTest;

public readonly record struct MediatorRequest(int Value) : IRequest<int>;
public readonly record struct MediatorRequestWithoutHandler() : IRequest<int>;

public class MediatorRequestHandler : IReceiver<MediatorRequest, int>
{
    public Task<int> ReceiverAsync(MediatorRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Value);
    }
}

public class MediatorWithResponseTest
{
    [Fact]
    public async Task SendAsync_ShouldReturnTrue_WhenCalled()
    {
        // arrange 
        var serviceProvider = new ServiceCollection()
            .AddMicrolisR(typeof(MediatorWithResponseTest))
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new MediatorRequest();
        
        // act
        var result = () =>  mediator.SendAsync(request, CancellationToken.None);

        // assert
        await result.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task SendAsync_ShouldReturnValueOfRequest_WhenCalled()
    {
        // arrange 
        var serviceProvider = new ServiceCollection()
            .AddMicrolisR(typeof(MediatorWithResponseTest))
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new MediatorRequest(4712);

        // act
        var result = await mediator.SendAsync(request, CancellationToken.None);

        // assert
        result.Should().Be(4712);
    }
    
    [Fact]
    public async Task SendAsync_ShouldThrow_WhenNoHandlerFound()
    {
        // arrange 
        var serviceProvider = new ServiceCollection()
            .AddMicrolisR(typeof(MediatorWithResponseTest))
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new MediatorRequestWithoutHandler();

        // act
        var result = () => mediator.SendAsync(request, CancellationToken.None);

        // assert
        await result.Should().ThrowAsync<Exception>();
    }
}