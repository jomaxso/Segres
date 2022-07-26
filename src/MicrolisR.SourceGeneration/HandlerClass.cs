namespace MicrolisR.SourceGeneration;

internal class HandlerClass
{
    public HandlerClass(string handlerFullName, string requestFullName)
    {
        HandlerFullName = handlerFullName;
        RequestFullName = requestFullName;
    }

    public string HandlerFullName { get; }
    public string RequestFullName { get; }
}