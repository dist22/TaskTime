using Microsoft.AspNetCore.Mvc;
using TaskManager.Dtos;
using TaskManager.Models;

namespace TaskManager.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    public Task Add(UserForRegistration userForRegistration);
    public Task<User> GetByEmail(string userEmail);
    public Task<bool> FindEmail(string userForRegistrationEmail);
    public Task<IEnumerable<User>> GetAllUsers();
    public Task<User> GetById(int userId);
    public Task AddTaskToUser(User user, TaskTime task);
    public Task EditUser(UserForEdit userForEdit);
}