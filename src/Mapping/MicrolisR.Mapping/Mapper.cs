using System.Reflection;
using MicrolisR.Mapping.Abstractions;
using MicrolisR.Mapping.internals;

namespace MicrolisR.Mapping;

public sealed class Mapper : IMapper
{
    private readonly List<MappableObject> _generatedMappers = new();

    public Mapper() 
        : this(Assembly.GetCallingAssembly())
    {
    }

    public Mapper(params Type[] markerTypes) 
        : this(markerTypes.Select(x => x.Assembly).ToArray())
    {
    }

    public Mapper(params Assembly[] markerAssemblies)
    {
        if (markerAssemblies.Any() is false)
            return;
        
        foreach (var markerAssembly in markerAssemblies)
            RegisterMapper(markerAssembly);
    }

    public T? Map<T>(object? value)
        where T : new()
    {
        if (value is null)
            return default;

        var target = value.GetType();
        var source = typeof(T);

        for (var i = 0; i < _generatedMappers.Count; i++)
        {
            var mappable = _generatedMappers[i];
            
            if(mappable.Target != target || mappable.Source != source)
                continue;

            return mappable.Handler.Handle<T>(value);
        }

        return default;
    }
    
    public TOut? Map<TIn, TOut>(TIn value)
        where TOut : new()
        where TIn : new()
    {
        if (value is null)
            return default;

        for (var i = 0; i < _generatedMappers.Count; i++)
        {
            if (_generatedMappers[i].Handler is IMapperDefinition<TIn, TOut> mapper)
                return mapper.Map(value);
        }

        return default;
    }
    
    public IEnumerable<TValue> MapMany<TValue>(IEnumerable<object>? targets) 
        where TValue : new()
    {
        var values = targets?
            .Select(target => Map<TValue>(target)!)
            .Where(value => value is not null);
        
        return values ?? Enumerable.Empty<TValue>();
    }
    
    public IEnumerable<TValue> MapMany<TSource, TValue>(IEnumerable<TSource>? targets) 
        where TValue : new() 
        where TSource : new()
    {
        var values = targets?
            .Select(target => Map<TSource, TValue>(target)!)
            .Where(value => value is not null);
        
        return values ?? Enumerable.Empty<TValue>();
    }

    internal TValue CheckAndGet<TSource, TValue>(TSource value) where TValue : new() where TSource : new()
    {
        if (_generatedMappers.Any(x => x.IsHandler(typeof(TSource), typeof(TValue))) is false)
        {
            RegisterMapper(typeof(TSource).Assembly);
        }

        return this.Map<TSource, TValue>(value)!;
    }

    private void RegisterMapper(Assembly markerAssembly)
    {
        var mappableHandlerTypes = markerAssembly.DefinedTypes.Where(type =>
        {
            var isMappableHandler = type.GetInterfaces()
                .Any(x => x == typeof(IMapperHandler));

            return isMappableHandler && type.IsAbstract is false && type.IsInterface is false;
        });

        foreach (var handlerType in mappableHandlerTypes)
        {
            var genericMapperTypes = handlerType.GetInterfaces()
                .Where(type =>
                    type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IMapperDefinition<,>))
                .Select(x =>
                {
                    var arguments = x.GetGenericArguments();
                    return (arguments[0], arguments[1]);
                });

            foreach (var genericMapperType in genericMapperTypes)
            {
                var mapperHandler = Activator.CreateInstance(handlerType, this) as IMapperHandler;

                if (_generatedMappers.Exists(x => x.Handler.Equals(mapperHandler)) || mapperHandler is null)
                    continue;
                var handler = new MappableObject(genericMapperType.Item1, genericMapperType.Item2, mapperHandler);
                _generatedMappers.Add(handler);
            }
        }
    }

}
