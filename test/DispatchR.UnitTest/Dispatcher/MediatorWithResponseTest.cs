using DispatchR.Contracts;
using DispatchR.Extensions.DependencyInjection.Microsoft;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DispatchR.UnitTest.Dispatcher;

public readonly record struct Mediator(int Value) : IQuery<int>;
public readonly record struct MediatorWithoutHandler() : IQuery<int>;

public class MediatorHandler : IQueryHandler<Mediator, int>
{
    public Task<int> HandleAsync(Mediator request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Value);
    }
}
public readonly record struct NotificationWithoutResponse(int Value) : IEvent;
public readonly record struct NotificationWithoutHandlerAndResponse() : IEvent;

public class RequestWithoutResponseHandler : IEventHandler<NotificationWithoutResponse>
{
    public Task HandleAsync(NotificationWithoutResponse notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class MediatorWithResponseTest
{
    [Fact]
    public async Task SendAsync_ShouldReturnTrue_WhenCalled()
    {
        // arrange 
        var serviceProvider = new ServiceCollection()
            .AddDispatchR(typeof(MediatorWithResponseTest))
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IDispatcher>();
        var request = new Mediator();
        
        // act
        var result = () =>  mediator.QueryAsync(request, CancellationToken.None);

        // assert
        await result.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task SendAsync_ShouldReturnValueOfRequest_WhenCalled()
    {
        // arrange 
        var serviceProvider = new ServiceCollection()
            .AddDispatchR(typeof(MediatorWithResponseTest))
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IDispatcher>();
        var request = new Mediator(4712);

        // act
        var result = await mediator.QueryAsync(request, CancellationToken.None);

        // assert
        result.Should().Be(4712);
    }
    
    [Fact]
    public async Task SendAsync_ShouldThrow_WhenNoHandlerFound()
    {
        // arrange 
        var serviceProvider = new ServiceCollection()
            .AddDispatchR(typeof(MediatorWithResponseTest))
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IDispatcher>();
        var request = new MediatorWithoutHandler();

        // act
        var result = () => mediator.QueryAsync(request, CancellationToken.None);

        // assert
        await result.Should().ThrowAsync<Exception>();
    }
}