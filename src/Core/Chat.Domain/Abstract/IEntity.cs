namespace Chat.Domain.Abstract;

public interface IEntity<T> where T : struct
{
    public T Id { get; set; }
}