namespace Chat.Domain.Abstract;

public abstract class EntityBase<T> where T : struct
{
    public T Id { get; set; }
}