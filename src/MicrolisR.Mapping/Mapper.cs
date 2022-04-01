using System.Reflection;

namespace MicrolisR.Mapping;

public class Mapper : IMapper
{
    private readonly List<MappableObject> _mappers = new();

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
            var mappableHandlerTypes = markerAssembly.ExportedTypes.Where(type =>
            {
                var isMappableHandler = type.GetInterfaces()
                    .Any(x => x == typeof(IMapperHandler));

                return isMappableHandler && type.IsAbstract is false && type.IsInterface is false;
            }).ToList();

            foreach (var handlerType in mappableHandlerTypes)
            {
                var genericMapperTypes = handlerType.GetInterfaces()
                    .Where(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IMapper<,>))
                    .Select(x =>
                    {
                        var arguments = x.GetGenericArguments();
                        return (arguments[0], arguments[1]);
                    })
                    .ToList();

                foreach (var genericMapperType in genericMapperTypes)
                {
                    var mapperHandler = Activator.CreateInstance(handlerType) as IMapperHandler;
                    if (_mappers.Exists(x => x.Handler.Equals(mapperHandler)) || mapperHandler is null)
                        continue;

                    var handler = new MappableObject(genericMapperType.Item1, genericMapperType.Item2, mapperHandler);
                    _mappers.Add(handler);
                }
            }
        }
    }

    public  bool TryMap<T>(object value, out T output)
        where T : class, new()
    {
        var result = InternalMap<T>(value)!;
        return (output = result) is not null;
    }
    
    public T Map<T>(object value)
        where T : class, new()
    {
        return InternalMap<T>(value) ?? throw new Exception();
    }

    public IEnumerable<T> MapMany<T>(IEnumerable<object> values)
        where T : class, new()
    {
        foreach (var value in values)
            yield return Map<T>(value);
    }
    
    private T? InternalMap<T>(object? value)
        where T : class, new()
    {
        if (value is null)
            return null;

        var target = value.GetType();
        var source = typeof(T);

        foreach (var mapper in _mappers)
        {
            if (mapper.IsHandler(source, target) is false)
                continue;

            return mapper.Handler.Handle<T>(value);
        }

        return null;
    }
}