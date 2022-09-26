using FluentAssertions;
using Xunit;

namespace MicrolisR.UnitTest.Sender;

internal readonly record struct Request(int Value) : IQuery<int>;
internal readonly record struct WithoutHandler() : IQuery<int>;

internal class Handler : IQueryHandler<Request, int>
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
        IQuerySender sender = new MicrolisR.Dispatcher(typeof(SenderWithResponseTest));
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
        IQuerySender sender = new MicrolisR.Dispatcher(typeof(SenderWithResponseTest));
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
        IQuerySender sender = new MicrolisR.Dispatcher(typeof(SenderWithResponseTest));
        var request = new WithoutHandler();

        // act
        var result = () => sender.SendAsync(request, CancellationToken.None);

        // assert
        await result.Should().ThrowAsync<Exception>();
    }
}