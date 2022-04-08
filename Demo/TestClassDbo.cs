using ClassLibrary2;
using MicrolisR.Mapping;
using MicrolisR.Mapping.Attributes;

namespace ClassLibrary1;

[Mappable(typeof(Employee))]
public class EmployeeDbo
{
    public int Id { get; set; }
    
    public string Firstname { get; set; }
    
    public string Lastname { get; set; }
    
    public JobDbo Job { get; set; }
}

[Mappable(typeof(Job))]
public class JobDbo
{
    [MappableIgnore]
    public int Id { get; set; }
    
    [MappableOptions(nameof(Job.Id))]
    public Guid JobId { get; set; }

    public string Titel { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
}

internal class GuidToStringMapper : MapperDefinition<Guid, string>
{
    public override string Map(Guid mappable)
    {
        throw new NotImplementedException();
    }

    public GuidToStringMapper(IMapper mapper) : base(mapper)
    {
    }
}



internal class StringToGuidMapper : MapperDefinition<string, Guid>
{
    public override Guid Map(string mappable)
    {
        throw new NotImplementedException();
    }

    public StringToGuidMapper(IMapper mapper) : base(mapper)
    {
    }
}