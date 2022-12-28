namespace Segres.Definitions;

internal interface IHandlerDefinition<out TSelf>
{
    public static abstract TSelf Create(Type requestType);
    
    public Type HandlerType { get; }
}