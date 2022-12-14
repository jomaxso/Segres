namespace Segres.AspNet;

internal interface IEndpointConfiguration
{
    void Configure(EndpointDefinition builder);
}