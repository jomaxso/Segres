using FluentAssertions;
using Xunit;

namespace MicrolisR.UnitTest;

internal readonly record struct MediatorRequest(int Value) : IRequestable<int>;
internal readonly record struct MediatorRequestWithoutHandler() : IRequestable<int>;

internal class MediatorRequestHandler : IRequestHandler<MediatorRequest, int>
{
    public Task<int> HandleAsync(MediatorRequest request, CancellationToken cancellationToken)
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
        object ServiceResolver(Type type) => type == typeof(MediatorRequestHandler) ? new MediatorRequestHandler() : throw new Exception();
        
        var handlerDetails = new Dictionary<Type, Type>()
        {
            {typeof(MediatorRequest), typeof(MediatorRequestHandler)}
        };

        var sender = new Sender(ServiceResolver, handlerDetails);
        IMediator mediator = new Mediator(sender);
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
        object ServiceResolver(Type type) => type == typeof(MediatorRequestHandler) ? new MediatorRequestHandler() : throw new Exception();
        
        var handlerDetails = new Dictionary<Type, Type>()
        {
            {typeof(MediatorRequest), typeof(MediatorRequestHandler)}
        };

        var sender = new Sender(ServiceResolver, handlerDetails);
        IMediator mediator = new Mediator(sender);
        
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
        object ServiceResolver(Type type) => type == typeof(MediatorRequestHandler) ? new MediatorRequestHandler() : throw new Exception();
        
        var handlerDetails = new Dictionary<Type, Type>()
        {
            {typeof(MediatorRequest), typeof(MediatorRequestHandler)}
        };

        var sender = new Sender(ServiceResolver, handlerDetails);
        IMediator mediator = new Mediator(sender);
        
        var request = new MediatorRequestWithoutHandler();

        // act
        var result = () => mediator.SendAsync(request, CancellationToken.None);

        // assert
        await result.Should().ThrowAsync<Exception>();
    }
}