using ClassLibrary2;
using MicrolisR.Mapping;

namespace ClassLibrary1;

public interface IRepository
{
    TestClass CreateTestClass(long item1, long item2, long text);
}

internal class Repo : IRepository
{
    private readonly IMapper _mapper;

    public Repo(IMapper mapper)
    {
        _mapper = mapper;
    }

    public TestClass CreateTestClass(long item1, long item2, long text)
    {
        var x = new TestClassDto()
        {
            Test = item1,
            Test2 = item2,
            Test3 = text
        };
        
        return _mapper.Map<TestClassDto, TestClass>(x);
    }
}