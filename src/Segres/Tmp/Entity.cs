using System.Diagnostics.CodeAnalysis;

namespace Segres.Tmp;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : ValueObject, IEntityId<TId>
{
    protected Entity()
    {
    }
    
    [SetsRequiredMembers]
    protected Entity(TId id)
    {
        Id = id;
    }

    public required TId Id { get; init; }

    /// <inheritdoc />
    public bool Equals(Entity<TId>? other)
        => other is not null && Id.Equals(other.Id);

    /// <inheritdoc />
    public override bool Equals(object? obj)
        => obj is Entity<TId> entity && Equals(entity);

    /// <inheritdoc />
    public override int GetHashCode()
        => Id.GetHashCode();
}