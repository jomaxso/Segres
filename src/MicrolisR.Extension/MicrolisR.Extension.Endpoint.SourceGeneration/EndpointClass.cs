namespace  MicrolisRExtension.Endpoint.SourceGeneration;

internal class EndpointClass
{
    public EndpointClass(string requestClassFullName, string responseClassFullName)
    {
        RequestClassFullName = requestClassFullName;
        ResponseClassFullName = responseClassFullName;
    }

    public string RequestClassFullName { get; }
    public string ResponseClassFullName { get; }
}