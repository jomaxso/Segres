using MicrolisR.Mapping.Attributes;

namespace MicrolisR.Mapping.Benchmark.Models;

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