using Segres.Abstractions;

namespace Segres;

public class ThePerson2 : IRequest
{
    public ThePerson2(int Age)
    {
        this.Age = Age;
    }

    public int Age { get; set; }
}
