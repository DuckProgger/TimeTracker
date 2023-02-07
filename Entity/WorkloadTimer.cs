using Entity.Base;

namespace Entity;

public class WorkloadTimer : EntityBase
{
    public Work Work { get; set; }
    public int WorkId { get; set; }
    public DateTime StartRecording { get; set; }
}