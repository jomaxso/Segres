using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MicrolisR.Internal;

public class Throw
{
    public static void IfNull<T>([NotNull] T? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);
    }
}