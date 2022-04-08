using MicrolisR.Api.Enumeration;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Reflection;

namespace MicrolisR.Api.Internals.Helper;

internal static class EnpointBindings
{
    private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> _bindings = new();

    internal static async ValueTask<object?> BindAsync(this HttpRequest httpRequest, Type type, ApiEndpointAttribute endpointAttribute)
    {
        if (_bindings.ContainsKey(type) is false)
            _bindings.AddOrUpdate(type, add => type.GetProperties().ToList(), (update, list) => list);

        var requestItemProperties = _bindings[type];


        if (endpointAttribute.Binding.HasFlag(BindContext.Default))
            return await httpRequest.BindDefaultAsync(type, endpointAttribute.Kind, requestItemProperties, httpRequest.HttpContext.RequestAborted);

        object? result = null;

        if (endpointAttribute.Binding.HasFlag(BindContext.FromJsonBody))
            result ??= await httpRequest.BindFromBodyAsync(type, httpRequest.HttpContext.RequestAborted);

        if (endpointAttribute.Binding.HasFlag(BindContext.FromRoute))
        {
            List<object?> requestValues = new();
            foreach (var property in requestItemProperties)
            {
                var propertyValue = httpRequest.RouteValues[property.Name.ToLower()];

                var value = CastValue(propertyValue);

                if (result is not null)
                {
                    requestValues.Add(value);
                    property.SetValue(result, value);
                }
            }

            result ??= Activator.CreateInstance(type, requestValues.ToArray());
        }

        return result;
    }

    private static async ValueTask<object?> BindDefaultAsync(this HttpRequest httpRequest, Type type, RequestKind requestType, List<PropertyInfo>? properties, CancellationToken cancellationToken)
    {
        if (properties is null)
            return null;

        return requestType switch
        {
            RequestKind.HttpGet => httpRequest.BindFromRoute(type, properties, cancellationToken),
            RequestKind.HttpPost => await httpRequest.BindFromBodyAsync(type, cancellationToken),
            RequestKind.HttpPut => await httpRequest.BindFromBodyAndRouteAsync(type, properties, cancellationToken),
            RequestKind.HttpDelete => httpRequest.BindFromRoute(type, properties, cancellationToken),
            _ => null,
        };
    }

    private static object? BindFromRoute(this HttpRequest httpRequest, Type type, List<PropertyInfo> properties, CancellationToken cancellationToken)
        => httpRequest.RouteValues.BindFromRoute(type, properties, cancellationToken);

    private static object? BindFromRoute(this IDictionary<string, object?> routeValues, Type type, List<PropertyInfo> properties, CancellationToken cancellationToken)
    {
        List<object> requestValues = new();

        foreach (var property in properties)
        {
            var propertyValue = routeValues[property.Name.ToLower()];
            var value = CastValue(propertyValue);

            if (value is not null)
                requestValues.Add(value);
        }

        return Activator.CreateInstance(type, requestValues.ToArray());
    }

    private static async Task<object?> BindFromBodyAsync(this HttpRequest httpRequest, Type type, CancellationToken cancellationToken)
    {
        try
        {
            return await httpRequest.ReadFromJsonAsync(type, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static async Task<object?> BindFromBodyAndRouteAsync(this HttpRequest httpRequest, Type type, List<PropertyInfo> properties, CancellationToken cancellationToken)
    {
        var requestObject = await httpRequest.BindFromBodyAsync(type, cancellationToken);

        foreach (var property in properties)
        {
            var propertyValue = httpRequest.RouteValues[property.Name.ToLower()];
            var value = CastValue(propertyValue);

            if (value is not null && requestObject is not null)
                property.SetValue(requestObject, value);
        }

        return requestObject;
    }

    private static object? CastValue(object? value)
    {
        if (value is null)
            return null;

        var stringValue = value.ToString();

        if (int.TryParse(stringValue, out int intValue))
            return intValue;
        else if (Guid.TryParse(stringValue, out Guid guidValue))
            return guidValue;

        return stringValue;
    }
}
