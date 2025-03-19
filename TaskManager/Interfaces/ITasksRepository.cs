using TaskManager.Dtos;
using TaskManager.Models;

namespace TaskManager.Interfaces;

public interface ITasksRepository : IBaseRepository<TaskTime>
{
    public Task Add(TaskTime createTask, Category category);

    public Task ChangeCategory(TaskTime task, Category category);

    public Task<IEnumerable<TaskTime>> GetAllTasks();

    public Task<TaskTime> GetById(int taskId);

    public Task AddUserToCategory(User user, TaskTime task);

    public Task<IEnumerable<TaskTime>> GetActive();

    public Task<IEnumerable<TaskTime>> GetByCategory(string categoryName);

    public Task<IEnumerable<TaskTime>> GetUserTask(int userId);

}