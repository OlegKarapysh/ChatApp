using System.Linq.Expressions;
using Chat.Domain.Abstract;

namespace Chat.DomainServices.Repositories;

public interface IRepository<T, TId>
    where T : EntityBase<TId>
    where TId : struct
{
    Task<T?> GetByIdAsync(TId id);
    Task<IList<T>> GetAllAsync();
    Task<IList<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task<bool> RemoveAsync(TId id);
    void Update(T entity);
}