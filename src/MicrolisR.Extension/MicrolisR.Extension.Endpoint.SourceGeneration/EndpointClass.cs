using System;
using System.Collections.Generic;

namespace  MicrolisRExtension.Endpoint.SourceGeneration;

internal class EndpointClass
{
    public EndpointClass(string requestClassFullName, string responseClassFullName, IEnumerable<Property> properties)
    {
        RequestClassFullName = requestClassFullName;
        ResponseClassFullName = responseClassFullName;
        Properties = properties;
    }

    public string RequestClassFullName { get; }
    public string ResponseClassFullName { get; }

    public IEnumerable<Property> Properties { get; }

    public string RequestAsName => RequestClassFullName.Replace(".", string.Empty);
    public string ResponseAsName => RequestClassFullName.Replace(".", string.Empty);
}

internal record class Property
{
    public string? FromAttribute { get; set; }
    public  string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}