using System;
using Entity;

namespace UI.Model;

internal class WorkModel : ModelBase
{
    public int Id { get; set; }

    public string Name { get; set; }

    public TimeSpan Workload { get; set; }

    public bool IsActive { get; set; }

    public TimeSpan? TimeElapsed => startRecording.HasValue
        ? DateTime.Now - startRecording
        : TimeSpan.MinValue;

    public static WorkModel Map(Work work)
    {
        return new WorkModel
        {
            Id = work.Id,
            Name = work.Name,
            Workload = work.Workload,
            IsActive = work.WorkloadTimer != null,
            startRecording = work.WorkloadTimer?.StartRecording
        };
    }

    private DateTime? startRecording;
}