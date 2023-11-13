using System.Linq.Expressions;
using Chat.Domain.Abstract;
using Chat.Domain.Enums;

namespace Chat.DomainServices.Repositories;

public interface IRepository<T, TId>
    where T : IEntity<TId>
    where TId : struct
{
    Task<T?> GetByIdAsync(TId id);
    Task<IList<T>> GetAllAsync();
    IQueryable<T> SearchWhere<TSearch>(string? searchFilter);
    IQueryable<T> ToSortedPage(string sortingProperty, SortingOrder sortingOrder, int page, int pageSize);
    Task<IList<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task<bool> RemoveAsync(TId id);
    void Update(T entity);
}