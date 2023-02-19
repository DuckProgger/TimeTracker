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

    public TimeSpan TotalWorkload
    {
        get
        {
            return Works.Any() 
                ? Works.Select(w => w.Workload).Aggregate((wl1, wl2) => wl1 + wl2) 
                : TimeSpan.Zero;
        }
    }

    public void AddWork(Work work)
    {
        var workWithSameNameExist = works.Any(w => w.Name == work.Name);
        if (workWithSameNameExist)
            throw new Exception($"Work with name {work.Name} already exist.");

        works.Add(work);
    }

    public void StartRecording(int workId, DateTime now)
    {
        var activeWorkExist = works.Any(w => w.IsActive);
        if (activeWorkExist)
        {
            var activeWorkName = works.First(w => w.IsActive).Name;
            throw new Exception($"There is already an active work with the name {activeWorkName} in the list of works");
        }

        var work = works.FirstOrDefault(w => w.Id == workId);
        if (work == null)
            throw new Exception($"Work with Id = {workId} not exist");

        work.StartRecording(now);
    }

    public void StopRecording(int workId, DateTime now)
    {
        var work = works.FirstOrDefault(w => w.Id == workId);
        if (work == null)
            throw new Exception($"Work with Id = {workId} not exist");

        work.StopRecording(now);
    }
}