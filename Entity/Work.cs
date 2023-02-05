using Entity.Base;

namespace Entity;

public class Work : NamedEntity
{
    public double Time { get; set; }

    public Work(string name)
    {
        Name = name;
    }
}