using Chat.Domain.Abstract;

namespace Chat.DomainServices.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<T, TId> GetRepository<T, TId>() where T : EntityBase<TId> where TId : struct;
    Task SaveChangesAsync(CancellationToken token = default);
}