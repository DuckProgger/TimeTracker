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

    public bool IsActive => WorkloadTimer != null;

    public WorkloadTimer? WorkloadTimer { get; private set; }

    public Work(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Work name can't be null or empty.");

        Name = name;
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Work name can't be null or empty.");

        Name = name;
    }

    public void AddWorkload(TimeSpan workload)
    {
        if (Workload + workload < TimeSpan.Zero)
            throw new Exception("Workload can't be less then zero");

        Workload += workload;
    }

    public void ChangeWorkload(TimeSpan newWorkload)
    {
        if (newWorkload < TimeSpan.Zero)
            throw new Exception("Workload can't be less then zero");

        Workload = newWorkload;
    }

    public void StartRecording(DateTime now)
    {
        if (IsActive)
            throw new Exception($"Work with Id = {Id} already active.");

        WorkloadTimer = new();
        WorkloadTimer.StartRecording(Id, now);
    }

    public void StopRecording(DateTime now)
    {
        if (!IsActive)
            throw new Exception($"Work with Id = {Id} is not active.");

        var elapsedTime = WorkloadTimer.GetElapsedTime(now);
        AddWorkload(elapsedTime);
        WorkloadTimer = null;
    }
}