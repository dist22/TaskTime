using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Dtos;
using TaskManager.Interfaces;
using TaskManager.Models;


namespace TaskManager.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public UserRepository(DataContextEF entity, IMapper mapper, IPasswordHasher passwordHasher) : base(entity)
    {
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public async Task Add(UserForRegistration userForRegistration)
    {
        User user = new User
        {
            Email = userForRegistration.Email,
            DisplayName = userForRegistration.DisplayName,
            PasswordHash = _passwordHasher.Genarate(userForRegistration.Password)
        };
        await _entity.AddAsync(user);
        if (!await SaveChanges())
            throw new Exception("Error while trying to save user");
    }

    public async Task<User> GetByEmail(string userEmail)
    {
        var user = await _entity
                       .AsNoTracking()
                       .FirstOrDefaultAsync(u => u.Email == userEmail) ??
                   throw new Exception("No user with this email was found.");
        return user;
        
    }

    public async Task<User> GetById(int userId)
    {
        return await _entity
                   .AsNoTracking()
                   .FirstOrDefaultAsync(u => u.UserId == userId)
               ?? throw new Exception("No user with this ID was found.");
    }

    public async Task<bool> FindEmail(string userForRegistrationEmail)
    {
        return await _entity.AnyAsync(u => u.Email == userForRegistrationEmail);
    }

    public async Task AddTaskToUser(User user, TaskTime task)
    {
        user.Tasks?.Add(task);
        if (!await Update(user))
            throw new Exception("Error while trying to update user");

    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _entity
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task EditUser(UserForEdit userForEdit)
    {
        var user = await GetById(userForEdit.UserId);
        user = _mapper.Map<User>(userForEdit);
        if (!await Update(user))
            throw new Exception("Error while trying to update user");
    }

    public async Task ChangePassword(User user, string newPassword)
    {
        user.PasswordHash = _passwordHasher.Genarate(newPassword);
        if (!await Update(user))
            throw new Exception("Error while trying to update user");
    }

}