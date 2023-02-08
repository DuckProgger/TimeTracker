using Entity.Base;

namespace Entity;

public class Work : NamedEntity
{
    /// <summary>
    /// Трудозатраты.
    /// </summary>
    public TimeSpan Workload { get; private set; }

    public Workday Workday { get; private set; }
    public int WorkdayId { get; private set; }

    public WorkloadTimer? WorkloadTimer { get; private set; }

    public Work(string name)
    {
        Name = name;
    }

    public void ChangeName(string name)
    {
        Name = name;
    }

    public void AddWorkload(TimeSpan workload)
    {
        Workload += workload;
    }

    public void ChangeWorkload(TimeSpan newWorkload)
    {
        Workload = newWorkload;
    }

    public void StartRecording()
    {
        WorkloadTimer = new();
        WorkloadTimer.StartRecording(Id);
    }

    public void StopRecording()
    {
        if (WorkloadTimer == null)
            throw new Exception($"Work with Id = {Id} is not active.");
        var elapsedTime = WorkloadTimer.GetElapsedTime();
        AddWorkload(elapsedTime);
        WorkloadTimer = null;
    }
}