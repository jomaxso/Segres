using DispatchR.Contracts;
using DispatchR.Extensions.DependencyInjection.Microsoft;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DispatchR.UnitTest.Dispatcher;

public readonly record struct MediatorWithoutResponseRequest() : ICommand;
public readonly record struct MediatorWithoutResponseRequestWithoutHandler() : ICommand;

public class MediatorWithoutResponseHandler : ICommandHandler<MediatorWithoutResponseRequest>
{
    public Task HandleAsync(MediatorWithoutResponseRequest request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class MediatorWithoutResponseTest
{
    [Fact]
    public async Task SendAsync_ShouldReturnTrue_WhenCalled()
    {
        // arrange 
        var serviceProvider = new ServiceCollection()
            .AddDispatchR(typeof(MediatorWithResponseTest))     
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IDispatcher>();
        var request = new MediatorWithoutResponseRequest();
        
        // act
        var result = () =>  mediator.CommandAsync(request, CancellationToken.None);

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
        var request = new MediatorWithoutResponseRequest();

        // act
        var result = () => mediator.CommandAsync(request, CancellationToken.None);

        // assert
        await result.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task SendAsync_ShouldThrow_WhenNoHandlerFound()
    {
        // arrange 
        var serviceProvider = new ServiceCollection()
            .AddDispatchR(typeof(MediatorWithResponseTest))
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IDispatcher>();
        var request = new MediatorWithoutResponseRequestWithoutHandler();

        // act
        var result = () => mediator.CommandAsync(request, CancellationToken.None);

        // assert
        await result.Should().ThrowAsync<Exception>();
    }
}