﻿using Demo;
using MicrolisR.Data.Mapping;

namespace Test;

[Mappable(typeof(Class2))]
public class TestClass 
{
    public int MyProperty { get; set; }
    public int MyProperty2 { get; set; }
    public int MyProperty3 { get; set; }
}