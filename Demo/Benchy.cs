using AutoMapper;
using BenchmarkDotNet.Attributes;

namespace Demo;

[MemoryDiagnoser]
public class Benchy
{
    private static readonly MicrolisR.Mapping.IMapper Mapper = new MicrolisR.Mapping.Mapper();

    private static readonly MapperConfiguration Config = new(cfg => cfg.CreateMap<TestClass, TestClassDto>());
    private static readonly AutoMapper.IMapper AutoMapper = new AutoMapper.Mapper(Config);

    private static readonly TestClass TestClass = new()
    {
        Test = 4711,
        Test2 = 300,
        Confiured = 17
    };
    
    [Benchmark]
    public TestClassDto Classic_Mapping()
    {
        var dto = new TestClassDto()
        {
            Test = TestClass.Test,
            Test2 = TestClass.Test2,
            Test3 = TestClass.Confiured
        };
    
        return dto;
    }
    
    [Benchmark]
    public TestClassDto MicrolisR_Mapping()
    {
        var dto = Mapper.Map<TestClassDto>(TestClass);
        return dto;
    }
    
    [Benchmark]
    public TestClassDto AutoMapper_Mapping()
    {
        var dto = AutoMapper.Map<TestClassDto>(TestClass);
        return dto;
    }
}