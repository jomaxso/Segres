// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Segres;
using Segres.Benchmarks;

// var b = new Benchmarks();
// b.GlobalSetup();
//
// var x = await b.CommandAsync_WithResponse_Segres();

BenchmarkRunner.Run<Benchmarks>();
return;


var guid1 = Guid.NewGuid();
var family = Family.Create("Sorge");


var p1 = new Person
{
    FamilyId = family.Id,
    Age = 17
};

var p2 = new Person
{
    FamilyId = family.Id,
    Age = 19
};


Console.WriteLine("Person: " + p1.Equals(p2));
Console.WriteLine("Family: " + p1.FamilyId.Equals(p2.FamilyId));


public class Family : Entity<FamilyId>
{
    public Family(FamilyId id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; }

    public static Family Create(string name)
    {
        return new Family(new FamilyId(Guid.NewGuid()), name);
    }
}

public record struct GetPersonAgeQuery(int Age) : IRequest<int>;

public class Person : Entity<PersonId>
{
    public Person(PersonId id)
        : base(id)
    {
    }

    public Person()
        : this(PersonId.Create())
    {
    }

    public FamilyId FamilyId { get; init; }

    public Age Age { get; init; }
}

public record Age : ValueObject
{
    public int Value { get; init; }

    public static implicit operator int(Age age)
    {
        return age.Value;
    }

    public static implicit operator Age(int age)
    {
        return new Age {Value = age};
    }
}

public record PersonId : Id<Guid>
{
    private PersonId(Guid value)
        : base(value)
    {
    }

    public static PersonId Create()
    {
        return new PersonId(Guid.NewGuid());
    }
}


public abstract record Id<T>(T Value)
{
}


public record FamilyId(Guid Value) : Id<Guid>(Value);


public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    protected Entity(TId id)
    {
        Id = id;
    }

    public TId Id { get; }

    public bool Equals(Entity<TId>? other)
    {
        return other is not null && Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Equals(entity);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}


public abstract record class ValueObject
{
}