using MicrolisR.Mapping.Abstractions;

namespace MicrolisR.Mapping.internals;

internal class MappableObject
{
    public MappableObject(Type source, Type target, IMapperHandler handler)
    {
        Source = source;
        Target = target;
        Handler = handler;
    }

    public Type Source { get; }
    public Type Target { get; }
    public IMapperHandler Handler { get; }

    public bool IsHandler(Type source, Type target)
    {
        return (this.Source == source && this.Target == target) ||
               (this.Source == target && this.Target == source);
    }
}