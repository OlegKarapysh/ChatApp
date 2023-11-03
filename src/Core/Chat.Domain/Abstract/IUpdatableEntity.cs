namespace Chat.Domain.Abstract;

public interface IUpdatableEntity
{
    public DateTime UpdatedAt { get; set; }
}