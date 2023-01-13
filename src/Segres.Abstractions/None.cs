namespace Segres;

/// <summary>
/// Represents the return type of void.
/// </summary>
public readonly record struct None 
{
    /// <summary>
    /// The representation of an empty result.
    /// </summary>
    public static readonly None Empty = new();
};