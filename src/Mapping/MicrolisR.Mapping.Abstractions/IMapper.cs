using System.Collections;

namespace MicrolisR.Mapping.Abstractions;

public interface IMapper
{
    T? Map<T>(object? target) 
        where T : new();
    
    IEnumerable<T> MapMany<T>(IEnumerable<object>? target) 
        where T : new();

    TOut? Map<TIn, TOut>(TIn value)
        where TOut : new()
        where TIn : new();
    
}
