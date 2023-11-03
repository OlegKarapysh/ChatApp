using Chat.Domain.Abstract;
using Chat.DomainServices.Repositories;

namespace Chat.DomainServices.UnitsOfWork;

public interface IUnitOfWork
{
    IRepository<T, TId> GetRepository<T, TId>() where T : IEntity<TId> where TId : struct;
    Task SaveChangesAsync(CancellationToken token = default);
    Task RollBackChangesAsync(CancellationToken token = default);
}