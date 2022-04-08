using MicrolisR.Mapping.Attributes;

namespace MicrolisR.Mapping.Benchmark.Models;

[Mappable(typeof(Employee))]
public class EmployeeDbo
{
    public int Id { get; set; }
    
    public string Firstname { get; set; }
    
    public string Lastname { get; set; }
    
    public JobDbo Job { get; set; }
}