using MicrolisR.Mapping.Attributes;
using MicrolisR.Mapping.Test.Entities;

namespace MicrolisR.Mapping.Test.Dbo;

[Mappable(typeof(Customer))]
public class CustomerDbo
{
    public string Surname { get; set; }
    public string Firstname { get; set; }
    public int Age { get; set; }
}

[Mappable(typeof(Customer))]
public class CustomerDboWithDifferentPrimitiveTypes
{
    public string Surname { get; set; }
    public string Firstname { get; set; }
    public double Age { get; set; }
}

[Mappable(typeof(Customer))]
public class CustomerDboWithNullableTypes
{
    public string? Surname { get; set; }
    public string? Firstname { get; set; }
    public int? Age { get; set; }
}