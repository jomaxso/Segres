namespace MicrolisR.Api.Enumeration;

[Flags]
public enum BindContext : byte
{
    Default = 0,
    FromRoute = 1,
    FromJsonBody = 2,
    FromQurey = 4,
    FromUser = 8
}
