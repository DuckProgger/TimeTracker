using System.ComponentModel.DataAnnotations;

namespace Entity.Base;

public class EntityBase
{
    public int Id { get; protected set; }
}