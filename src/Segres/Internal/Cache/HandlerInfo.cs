namespace Segres.Internal.Cache;

internal sealed class HandlerInfo
{
    public Delegate Method { get; }
    
    public HandlerInfo(Type type, Delegate method)
    {
        Type = type;
        Method = method;
    }

    public Type Type { get; }
    

    
    public TDelegate ResolveAsyncMethod<TDelegate>()
        where TDelegate : Delegate => (TDelegate)Method;
}