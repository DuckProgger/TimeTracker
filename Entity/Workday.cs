using System.Security.Cryptography.X509Certificates;
using Entity.Base;

namespace Entity;

public class Workday : EntityBase
{
    private readonly List<Work> works = new();

    public Workday(DateOnly date)
    {
        Date = date;
    }

    /// <summary>
    /// Дата работы.
    /// </summary>
    public DateOnly Date { get; private set; }

    public IReadOnlyCollection<Work> Works => works.AsReadOnly();

    public void AddWork(Work work)
    {
        works.Add(work);
    }
}