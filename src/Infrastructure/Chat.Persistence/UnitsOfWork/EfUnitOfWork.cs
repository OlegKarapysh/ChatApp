namespace Chat.Persistence.UnitsOfWork;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly ChatDbContext _dbContext;
    private readonly Dictionary<Type, object> _repositories = new();

    public EfUnitOfWork(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IRepository<T, TId> GetRepository<T, TId>()
        where T : class, IEntity<TId>
        where TId : struct
    {
        var entityType = typeof(T);
        if (_repositories.TryGetValue(entityType, out var repository))
        {
            return (IRepository<T, TId>)repository;
        }

        var repositoryType = typeof(EfRepository<T, TId>);
        var newRepository = Activator.CreateInstance(repositoryType, _dbContext);
        _repositories.Add(entityType, newRepository);

        return (IRepository<T, TId>)newRepository;
    }

    public async Task SaveChangesAsync(CancellationToken token = default)
    {
        await _dbContext.SaveChangesAsync(token);
    }

    public async Task RollBackChangesAsync(CancellationToken token = default)
    {
        await _dbContext.Database.RollbackTransactionAsync(token);
    }
}