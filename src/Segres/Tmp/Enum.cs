using System.Reflection;

namespace Segres.Tmp;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TSelf"></typeparam>
public abstract class Enum<TSelf> : IEquatable<Enum<TSelf>>
    where TSelf : Enum<TSelf>
{
    private static readonly IDictionary<int, TSelf> Enumerations = CreateEnumerations();

    public int Value { get; }
    public string Name { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="name"></param>
    protected Enum(int value, string name)
    {
        Value = value;
        Name = name;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enumeration"></param>
    /// <returns></returns>
    public static implicit operator string(Enum<TSelf> enumeration) => enumeration.Name;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="enumeration"></param>
    /// <returns></returns>
    public static implicit operator int(Enum<TSelf> enumeration) => enumeration.Value;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="AggregateException"></exception>
    public static TSelf FromValue(int value) 
        => Enumerations.TryGetValue(value, out var v) ? v : throw new AggregateException();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TSelf FromName(string value) 
        => Enumerations.Values.Single(x => x.Name == value);

    /// <inheritdoc />
    public bool Equals(Enum<TSelf>? other)
        => other is not null
           && GetType() == other.GetType()
           && Value == other.Value;

    /// <inheritdoc />
    public override bool Equals(object? obj)
        => obj is Enum<TSelf> other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
        => Value.GetHashCode();

    /// <inheritdoc />
    public override string ToString()
        => Name;

    private static IDictionary<int, TSelf> CreateEnumerations()
    {
        var enumType = typeof(TSelf);

        var fieldsForType = enumType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fieldInfo => enumType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => (TSelf) fieldInfo.GetValue(default)!);
        
        return fieldsForType.ToDictionary(x => x.Value);
    }
}