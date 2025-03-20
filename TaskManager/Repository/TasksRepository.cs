using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Dtos;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Repository;

public class TasksRepository : BaseRepository<TaskTime>, ITasksRepository
{

    public TasksRepository(DataContextEF entity) : base(entity)
    { }

    #region GET()

    public async Task<IEnumerable<TaskTime>> GetAllTasks()
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
                   throw new Exception("Task with given ID was not found.");
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

    public async Task<IEnumerable<TaskTime>> GetUserTask(int userId)
    {
        return await _entity
            .AsNoTracking()
            .Where(t => t.Users.Any(u => u.UserId == userId))
            .ToListAsync();
    }
    
    #endregion

    public async Task Add(TaskTime createTask, Category category)
    {
        
        createTask.CategoryId = category.CategoriesId;

        await _entity.AddAsync(createTask);

        if (!await SaveChanges())
            throw new Exception("Error while trying to save task");
        
        category.Tasks.Add(createTask);
        await Update(category);
        
    }

    public async Task ChangeCategory(TaskTime task, Category category)
    {
        task.CategoryId = category.CategoriesId;
        task.CategoryName = category.CategoriesName;

        if (!await Update(task))
            throw new Exception("Error while trying to update task");
        
        category.Tasks.Add(task);
        if (!await Update(category))
            throw new Exception("Error while trying to update task");
        

    }

    public async Task AddUserToCategory(User user, TaskTime task)
    {
        task.Users.Add(user);
        if (!await Update(task))
            throw new Exception("Error while trying to update task");
    }


}