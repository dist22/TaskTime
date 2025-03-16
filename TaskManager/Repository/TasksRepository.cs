using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Dtos;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Repository;

public class TasksRepository : BaseRepository<TaskTime>, ITasksRepository
{

    public TasksRepository(DataContextEF entity, IMapper mapper,
        IUserRepository userRepository) : base(entity)
    { }

    #region GET()

    public async Task<IEnumerable<TaskTime>> GetAll()
    {
        return await _entity
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TaskTime> GetById(int taskId)
    {
        var task = await _entity
                       .AsNoTracking()
                       .FirstOrDefaultAsync(t => t.TaskId == taskId) ??
                   throw new Exception("Task not found");
        return task;
    }

    public async Task<IEnumerable<TaskTime>> GetActive()
    {
        return await _entity
            .AsNoTracking()
            .Where(t => t.isComplete == false)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskTime>> GetByCategory(string categoryName)
    {
        return await _entity
            .AsNoTracking()
            .Where(t => t.CategoryName == categoryName)
            .ToListAsync();
    }

    #endregion

    public async Task Add(TaskTime createTask, Category category)
    {
        
        createTask.CategoryId = category.CategoriesId;

        await _entity.AddAsync(createTask);

        if (!await SaveChanges())
            throw new Exception("Fail");
        
        category.Tasks.Add(createTask);
        await Update(category);
        
    }

    public async Task ChangeCategory(TaskTime task, Category category)
    {
        task.CategoryId = category.CategoriesId;
        task.CategoryName = category.CategoriesName;

        if (!await Update(task))
            throw new Exception("fail");
        
        category.Tasks.Add(task);
        if (!await Update(category))
            throw new Exception("fail");
        

    }

    public async Task AddUserToCategory(User user, TaskTime task)
    {
        task.Users.Add(user);
        if (!await Update(task))
            throw new Exception("Fail");
    }

    public async Task<IEnumerable<TaskTime>> GetUserTask(int userId)
    {
        return await _entity
            .AsNoTracking()
            .Where(t => t.Users.Any(u => u.UserId == userId))
            .ToListAsync();
    }

}