using MicrolisR;
using MicrolisR.Abstractions;

namespace Demo.Lib;


internal class RequestHandler : IReceiver<Request, bool>
{
    public Task<bool> ReceiveAsync(Request request, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}



