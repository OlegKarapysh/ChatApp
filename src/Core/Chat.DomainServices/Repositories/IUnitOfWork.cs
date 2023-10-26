namespace Chat.DomainServices.Repositories;

public interface IUnitOfWork
{
    IRepository<T> GetRepository<T>() where T : class;
    Task SaveChangesAsync();
    void SaveChanges();
}