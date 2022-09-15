﻿using FluentAssertions;
using Xunit;

namespace MicrolisR.UnitTest.Sender;

internal readonly record struct Request(int Value) : IQueryRequest<int>;
internal readonly record struct RequestWithoutHandler() : IQueryRequest<int>;

internal class RequestHandler : IQueryRequestHandler<Request, int>
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
        ISender sender = new MicrolisR.Mediator(typeof(SenderWithResponseTest));
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
        ISender sender = new MicrolisR.Mediator(typeof(SenderWithResponseTest));
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
        ISender sender = new MicrolisR.Mediator(typeof(SenderWithResponseTest));
        var request = new RequestWithoutHandler();

        // act
        var result = () => sender.SendAsync(request, CancellationToken.None);

        // assert
        await result.Should().ThrowAsync<Exception>();
    }
}