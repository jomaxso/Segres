using FluentAssertions;
using Xunit;

namespace MicrolisR.UnitTest.Sending;

internal readonly record struct Request(int Value) : IRequestable<int>;
internal readonly record struct RequestWithoutHandler() : IRequestable<int>;

internal class RequestHandler : IRequestHandler<Request, int>
{
    public Task<int> HandleAsync(Request request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Value);
    }
}

public class SenderWithResponseTest
{
    [Fact]
    public async Task SendAsync_ShouldReturnTrue_WhenCalled()
    {
        // arrange 
        object ServiceResolver(Type type) => type == typeof(RequestHandler) ? new RequestHandler() : throw new Exception();
        
        var handlerDetails = new Dictionary<Type, Type>()
        {
            {typeof(Request), typeof(RequestHandler)}
        };

        ISender sender = new MicrolisR.Sender(ServiceResolver, handlerDetails);
        var request = new Request();
        
        // act
        var result = await sender.SendAsync(request, CancellationToken.None);

        // assert
        result.Should().Be(0);
    }
    
    [Fact]
    public async Task SendAsync_ShouldReturnValueOfRequest_WhenCalled()
    {
        // arrange 
        object ServiceResolver(Type type) => type == typeof(RequestHandler) ? new RequestHandler() : throw new Exception();
        
        var handlerDetails = new Dictionary<Type, Type>()
        {
            {typeof(Request), typeof(RequestHandler)}
        };

        ISender sender = new MicrolisR.Sender(ServiceResolver, handlerDetails);
        var request = new Request(4712);

        // act
        var result = await sender.SendAsync(request, CancellationToken.None);

        // assert
        result.Should().Be(4712);
    }
    
    [Fact]
    public async Task SendAsync_ShouldThrow_WhenNoHandlerFound()
    {
        // arrange 
        object ServiceResolver(Type type) => type == typeof(RequestHandler) ? new RequestHandler() : throw new Exception();
        
        var handlerDetails = new Dictionary<Type, Type>()
        {
            {typeof(Request), typeof(RequestHandler)}
        };

        ISender sender = new Sender(ServiceResolver, handlerDetails);
        var request = new RequestWithoutHandler();

        // act
        var result = () => sender.SendAsync(request, CancellationToken.None);

        // assert
        await result.Should().ThrowAsync<Exception>();
    }
}