namespace TaskManager.Interfaces;

public interface IBaseRepository<T> where T : class
{
    public Task<bool> SaveChanges();

    public Task<bool> RemoveEntity(T entity);

    public Task<bool> Update<U>(U entity) where U : class;
}