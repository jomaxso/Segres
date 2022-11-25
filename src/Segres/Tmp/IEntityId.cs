namespace Segres.Tmp;

public interface IConstructor<out TSelf>
{
    public static abstract TSelf Create();
}

public interface IEntityId<TSelf>
    where TSelf : IEntityId<TSelf>
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    public static abstract implicit operator Guid(TSelf identifier);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static abstract TSelf CreateUnique();
}


[EntityId]
public partial record MyEntityId;


public partial record MyEntityId
{
    private MyEntityId()
    {
        this.Value = Guid.NewGuid();
    }

    /// <inheritdoc />
    public Guid Value { get; }

    /// <inheritdoc />
    public static implicit operator Guid(MyEntityId identifier) => identifier.Value;

    public static MyEntityId CreateUnique() => new();
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class EntityIdAttribute : Attribute
{
}
