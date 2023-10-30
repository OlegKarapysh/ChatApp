namespace Chat.Domain.Abstract;

public abstract class AuditableEntityBase<T> : EntityBase<T>, ICreatableEntity, IUpdatableEntity where T : struct
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}