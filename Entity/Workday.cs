using Entity.Base;

namespace Entity;

public class Workday : EntityBase
{
    /// <summary>
    /// Дата работы.
    /// </summary>
    public DateOnly Date { get; set; }

    public ICollection<Work> Works { get; set; }
}