using Entity;

namespace UI.Model;

internal class WorkModel : ModelBase
{
    public int Id { get; set; }

    public string Name { get; set; }

    public double Time { get; set; }

    public static WorkModel Map(Work work)
    {
        return new WorkModel
        {
            Id = work.Id,
            Name = work.Name,
            Time = work.Time,
        };
    }
}