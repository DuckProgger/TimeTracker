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

    public void StartRecording(int workId)
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

        work.StartRecording();
    }

    public void StopRecording(int workId)
    {
        var work = works.FirstOrDefault(w => w.Id == workId);
        if (work == null)
            throw new Exception($"Work with Id = {workId} not exist");

        work.StopRecording();
    }
}