using Entity.Base;

namespace Entity;

public class Work : NamedEntity
{
    /// <summary>
    /// Трудозатраты.
    /// </summary>
    public double Workload { get; set; }

    public Workday Workday { get; set; }
    public int WorkdayId { get; set; }

    public Work(string name)
    {
        Name = name;
    }
}