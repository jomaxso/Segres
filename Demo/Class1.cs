using MicrolisR.Mapping;
using MicrolisR.Mapping.Attributes;

namespace Demo;

public class TestClass
{
    public int Test { get; set; }
    public int Test2 { get; set; }
    public int Confiured { get; set; }
}

[Mappable(typeof(TestClass))]
public partial class TestClassDto
{
    public int Test { get; set; }
    
    [MappableIgnore]
    public int Test2 { get; set; }
    
    [MappableOptions(nameof(TestClass.Confiured))]
    public int Test3 { get; set; }
}