namespace MicrolisR;

public sealed class Validator : IValidator
{
    private readonly KeyValuePair<Type, object>[] _handlerDetails;
    private readonly IValidationHandlerResolver[] _validationHandlerResolvers;

    public Validator(IEnumerable<KeyValuePair<Type, object>> handlerDetails)
    {
        _handlerDetails = handlerDetails.ToArray();
        _validationHandlerResolvers = FindAllHandlerResolver<IValidationHandlerResolver>(_handlerDetails);
    }

    
    public void Validate(IValidatable value)
    {
        var objectType = value.GetType();

        for (var i = 0; i < _validationHandlerResolvers.Length; i++)
        {
            for (var j = 0; j < _handlerDetails.Length; j++)
            {
                var handler = _handlerDetails[j];
                
                if (handler.Key != objectType)
                    continue;
                
                _validationHandlerResolvers[i].Resolve(handler.Value, value);
            }
        }
    }

    private static T[] FindAllHandlerResolver<T>(IEnumerable<KeyValuePair<Type, object>> details)
    {
        var requestHandlerResolvers = new List<T>();

        foreach (var detail in details)
        {
            var resolvers = detail.Value
                .GetType().Assembly
                .GetTypes()
                .Where(x => x.GetInterface(typeof(T).Name) is not null)
                .Select(Activator.CreateInstance)
                .Where(x => x is not null)
                .Cast<T>();
            
            requestHandlerResolvers.AddRange(resolvers);
        }

        return requestHandlerResolvers.DistinctBy(x => x!.GetType()).ToArray();
    }


}