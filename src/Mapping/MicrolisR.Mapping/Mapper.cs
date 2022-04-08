using System.Reflection;

namespace MicrolisR.Mapping;

public sealed class Mapper : IMapper
{
    private readonly List<MappableObject> _generatedMappers = new();

    public Mapper() : this(Assembly.GetCallingAssembly())
    {
       
    }

    public Mapper(params Type[] markerTypes) : this(markerTypes.Select(x => x.Assembly).ToArray())
    {
    }

    public Mapper(params Assembly[] markerAssemblies)
    {
        if (markerAssemblies.Any() is false)
            return;
        
        foreach (var markerAssembly in markerAssemblies)
        {
            RegisterMapper(markerAssembly);
        }
    }

    public T? Map<T>(object? value)
        where T : new()
    {
        T? result = default;

        if (value is null)
            return result;

        var target = value.GetType();
        var source = typeof(T);


        for (var i = 0; i < _generatedMappers.Count; i++)
        {
            var mappable = _generatedMappers[i];
            
            if(mappable.Target != target)
                continue;
            
            if (mappable.Source != source)
                continue;
            
            result = mappable.Handler.Handle<T>(value);
            break;
        }

        return result;
    }

    public TOut? Map<TIn, TOut>(TIn value)
        where TOut : new()
        where TIn : new()
    {
        TOut? result = default;

        if (value is null)
            return result;

        for (var i = 0; i < _generatedMappers.Count; i++)
        {
            var mappable = _generatedMappers[i];
            
            if (mappable.Handler is not IMapperDefinition<TIn, TOut> mapper)
                continue;
        
            result = mapper.Map(value);
            break;
        }

        return result;
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
                })
                .ToList();

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