using Entity.Base;

namespace Entity;

public class WorkloadTimer : EntityBase
{
    public Work Work { get; private set; }
    public int WorkId { get; private set; }
    public DateTime StartRecordingDateTime { get; private set; }

    public void StartRecording(int workId)
    {
        WorkId = workId;
        StartRecordingDateTime = DateTime.Now;
    }

    public TimeSpan GetElapsedTime()
    {
        return DateTime.Now - StartRecordingDateTime;
    }
}