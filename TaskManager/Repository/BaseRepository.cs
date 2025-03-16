using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Interfaces;

namespace TaskManager.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly DataContextEF _context;
    protected readonly DbSet<T> _entity;

    public BaseRepository(DataContextEF context)
    {
        _context = context;
        _entity = context.Set<T>();
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveEntity(T entity)
    {
        _entity.Remove(entity);
        return await SaveChanges();
    }

    public async Task<bool> Update<U>(U entity) where U: class
    {
        _context.Set<U>().Update(entity);
        return await SaveChanges();
    }

}