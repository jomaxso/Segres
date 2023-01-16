namespace Segres;


/// <summary>
/// Send a request to be handled by a single handler
/// or
/// Publish a notification to be handled by multiple handlers.
/// </summary>
public interface IMediator : ISender, IPublisher
{
}