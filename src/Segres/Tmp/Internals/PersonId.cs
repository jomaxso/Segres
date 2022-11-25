namespace Segres.Tmp;

public partial class PersonId : ValueObject, IEntityId<PersonId>
{
    private PersonId(Guid id)
    {
        this.Value = id;
    }

    /// <inheritdoc />
    public Guid Value { get; }


    /// <inheritdoc />
    public static implicit operator Guid(PersonId identifier) => identifier.Value;
    /// <inheritdoc />
    public static PersonId CreateUnique() => new(Guid.NewGuid());
}

public sealed class Address
{
    
}

[EntityId]
public sealed partial class PersonId
{
    
}

