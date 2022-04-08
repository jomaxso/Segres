namespace MicrolisR.Mapping.Abstractions;

public interface IMapperDefinition<in TSource, out TTarget> : IMapperHandler
{
    TTarget Map(TSource mappable);

    T? IMapperHandler.Handle<T>(object? value) 
        where T : default
    {
        if (value is not TSource source)
            return default;

        return Map(source) is T target ? target : default;
    }
}