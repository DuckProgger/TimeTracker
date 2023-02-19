using Entity.Base;

namespace Entity;

public class WorkloadTimer : EntityBase
{
    public Work Work { get; private set; }
    public int WorkId { get; private set; }
    public DateTime StartRecordingDateTime { get; private set; }

    public void StartRecording(int workId, DateTime now)
    {
        WorkId = workId;
        StartRecordingDateTime = now;
    }

    public TimeSpan GetElapsedTime(DateTime now)
    {
        return now - StartRecordingDateTime;
    }
}