using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MicrolisR.Utilities;

public static class Throw
{
    public static void IfNull<T>([NotNull] T? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);
    }
    
    public static void IfNullOrEmpty<T>([NotNull] IEnumerable<T>? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);

        if (argument.Count() < 1)
        {
            throw new ArgumentException("Argument is Empty. ", paramName);
        }
        
    }
}