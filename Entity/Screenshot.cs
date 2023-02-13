using Entity.Base;

namespace Entity;

public class Screenshot : EntityBase
{
    public Screenshot(byte[] data, DateTime created)
    {
        Data = data;
        Created = created;
    }

    public byte[] Data { get; private set; }

    public DateTime Created { get; private set; }
}