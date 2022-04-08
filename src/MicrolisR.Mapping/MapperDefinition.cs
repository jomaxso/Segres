namespace MicrolisR.Mapping;

public abstract class MapperDefinition<TSource, TTarget> : IMapperDefinition<TSource, TTarget>
{
    public abstract TTarget Map(TSource mappable);

    public T? Handle<T>(object value)
    {
        if (value is not TSource source)
            return default;
        
        return this.Map(source) is T sourceResult ? sourceResult : default;
    }
}