namespace Chat.Domain.Abstract;

public interface ICreatableEntity<T>
{
    public T Id { get; set; }
    public DateTime CreatedAt { get; set; }
}