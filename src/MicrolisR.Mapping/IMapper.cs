namespace MicrolisR.Mapping;

public interface IMapper<TMappable, TTarget> : IMapperHandler
    where TMappable : IMappable<TTarget>
    where TTarget : class, new()
{
    T? IMapperHandler.Handle<T>(object value) where T : class
    {
        return value switch
        {
            TMappable testClassDto => this.Map(testClassDto) as T,
            TTarget testClass => this.Map(testClass) as T,
            _ => null
        };
    }

    TTarget Map(TMappable mappable);
    TMappable Map(TTarget target);
}

public interface IMapper
{
    T Map<T>(object target) where T : class, new();

    bool TryMap<T>(object value, out T output)
        where T : class, new();

    IEnumerable<T> MapMany<T>(IEnumerable<object> values)
        where T : class, new();
}