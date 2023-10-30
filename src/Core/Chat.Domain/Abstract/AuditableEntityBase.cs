namespace Chat.Domain.Abstract;

public abstract class AuditableEntityBase<T> : EntityBase<T>, ICreatableEntity<T>, IUpdatableEntity<T> where T : struct
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}