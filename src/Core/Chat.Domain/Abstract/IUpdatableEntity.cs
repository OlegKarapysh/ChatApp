namespace Chat.Domain.Abstract;

public interface IUpdatableEntity<T>
{
    public T Id { get; set; }
    public DateTime UpdatedAt { get; set; }
}