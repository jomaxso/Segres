namespace Segres.Tmp.Http;

internal delegate ValueTask<T> HttpRequestDelegate<T>(object handler, IHttpRequest<T> request, CancellationToken cancellationToken);