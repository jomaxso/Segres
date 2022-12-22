using System.Diagnostics.CodeAnalysis;

namespace Segres;

public readonly record struct None
{
    public static readonly None Empty = new();
};