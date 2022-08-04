using FluentAssertions;
using Xunit;

namespace MicrolisR.UnitTest.Sending;

internal readonly record struct RequestWithoutResponse(int Value) : IRequestable;
internal readonly record struct RequestWithoutHandlerAndResponse() : IRequestable;

internal class RequestWithoutResponseHandler : IRequestHandler<RequestWithoutResponse>
{
    public Task HandleAsync(RequestWithoutResponse request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class SenderWithoutResponseTest
{
    [Fact]
    public async Task SendAsync_ShouldReturnTrue_WhenCalled()
    {
        // arrange 
        object ServiceResolver(Type type) => type == typeof(RequestWithoutResponseHandler) ? new RequestWithoutResponseHandler() : throw new Exception();
        
        var handlerDetails = new Dictionary<Type, Type>()
        {
            {typeof(RequestWithoutResponse), typeof(RequestWithoutResponseHandler)}
        };

        ISender sender = new MicrolisR.Sender(ServiceResolver, handlerDetails);
        var request = new RequestWithoutResponse();
        
        // act
        var result = () =>  sender.SendAsync(request, CancellationToken.None);

        // assert
        await result.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task SendAsync_ShouldReturnValueOfRequest_WhenCalled()
    {
        // arrange 
        object ServiceResolver(Type type) => type == typeof(RequestWithoutResponseHandler) ? new RequestWithoutResponseHandler() : throw new Exception();
        
        var handlerDetails = new Dictionary<Type, Type>()
        {
            {typeof(RequestWithoutResponse), typeof(RequestWithoutResponseHandler)}
        };

        ISender sender = new MicrolisR.Sender(ServiceResolver, handlerDetails);
        var request = new RequestWithoutResponse(4712);

        // act
        var result = () =>  sender.SendAsync(request, CancellationToken.None);

        // assert
        await result.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task SendAsync_ShouldThrow_WhenNoHandlerFound()
    {
        // arrange 
        object ServiceResolver(Type type) => type == typeof(RequestWithoutResponseHandler) ? new RequestWithoutResponseHandler() : throw new Exception();
        
        var handlerDetails = new Dictionary<Type, Type>()
        {
            {typeof(RequestWithoutResponse), typeof(RequestWithoutResponseHandler)}
        };

        ISender sender = new Sender(ServiceResolver, handlerDetails);
        var request = new RequestWithoutHandlerAndResponse();

        // act
        var result = () => sender.SendAsync(request, CancellationToken.None);

        // assert
        await result.Should().ThrowAsync<Exception>();
    }
}