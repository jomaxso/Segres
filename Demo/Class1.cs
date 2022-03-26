using MicrolisR.Data.Mapping;

namespace Demo;

[Mappable(typeof(Class2))]
public class Class1 
{
    public int MyProperty { get; set; }

    public void Ex()
    {
       
    }
}

public class Class2
{
    public int MyProperty { get; set; }

    public void Ex()
    {
        this.MyProperty++;  
        this.ToTest();
    }
    
}







