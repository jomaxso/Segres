using MicrolisR.Data.Mapping;

namespace Demo;


[Mappable(typeof(Class2))]
public class TestClass 
{
    public int Test { get; set; }
    public int Test2 { get; set; }
    public int Test3 { get; set; }
}