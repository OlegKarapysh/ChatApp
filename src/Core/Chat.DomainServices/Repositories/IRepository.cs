namespace Chat.DomainServices.Repositories;

public interface IRepository<T, TId>
    where T : IEntity<TId>
    where TId : struct
{
    Task<T?> GetByIdAsync(TId id);
    Task<IList<T>> GetAllAsync();
    IQueryable<T> SearchWhere<TSearch>(string? searchFilter);
    IQueryable<T> AsQueryable();
    IQueryable<T> ToSortedPage(string sortingProperty, SortingOrder sortingOrder, int page, int pageSize);
    Task<IList<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
    Task<T?> FindFirstAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task<bool> RemoveAsync(TId id);
    Task<bool> RemoveRangeAsync(IEnumerable<TId> entityIds);
    T Update(T entity);
}