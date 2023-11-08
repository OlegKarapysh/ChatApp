using System.Linq.Expressions;
using Chat.Domain.Abstract;
using Chat.DomainServices.Repositories;
using Chat.Persistence.Contexts;
using Chat.Persistence.UnitsOfWork;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.Repositories;

public sealed class EfRepository<T, TId> : IRepository<T, TId>
    where T : class, IEntity<TId>
    where TId : struct
{
    private readonly ChatDbContext _dbContext;

    public EfRepository(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(TId id) => await _dbContext.Set<T>().FindAsync(id);

    public async Task<IList<T>> GetAllAsync() => await _dbContext.Set<T>().ToListAsync();
    public IQueryable<T> GetAsQueryable() => _dbContext.Set<T>();

    public async Task<IList<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        => await _dbContext.Set<T>().Where(predicate).ToListAsync();

    public async Task AddAsync(T entity) => await _dbContext.Set<T>().AddAsync(entity);

    public void Update(T entity) => _dbContext.Set<T>().Update(entity);

    public async Task<bool> RemoveAsync(TId id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null)
        {
            return false;
        }
        
        _dbContext.Set<T>().Remove(entity);
        return true;
    }
}