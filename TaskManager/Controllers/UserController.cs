using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Dtos;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ITasksRepository _tasksRepository;
    private readonly IMapper _mapper;

    public UserController(IUserRepository userRepository, IMapper mapper, ITasksRepository tasksRepository)
    {
        _userRepository = userRepository;
        _tasksRepository = tasksRepository;
        _mapper = mapper;
    }

    #region GET()
    
    //Getting all users 1.GET()
    [HttpGet("GetUsers")]
    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        var user = await _userRepository.GetAllUsers();
        return _mapper.Map<IEnumerable<UserDto>>(user);
    }
    
    //Getting user by id 2.GET()
    [HttpGet("GetUser/{userId}")]
    public async Task<UserDto> GetUser(int userId)
    {
        User user = await _userRepository.GetById(userId);
        return _mapper.Map<UserDto>(user);
    }
    
    //Get user tasks 3.GET()
    [HttpGet("GetUserTask/{userId}")]
    public async Task<IEnumerable<TaskDto>> GetUserTasks(int userId)
    {
        var tasks = await _tasksRepository.GetUserTask(userId);
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    //Getting an authenticated user 4.GET()
    [HttpGet("GetMyUser")]
    public async Task<UserDto> GetMyUser()
    {
        User? user = await _userRepository.GetById(Int32.Parse(this.User.FindFirst("userId")?.Value));
        return _mapper.Map<UserDto>(user);
    }
    #endregion

    #region PUT()
    //User editing 5.PUT()
    [HttpPut("EditUser")]
    public async Task<IActionResult> EditUser(UserForEdit userForEdit)
    {
        await _userRepository.EditUser(userForEdit);
        return Ok();
    }
    #endregion 
    #region DELETE()

    //Deleting a user 6.DELETE()
    [HttpDelete("DeleteUser/{email}")]
    public async Task<IActionResult> DeleteUser(string email)
    {
        User? user = await _userRepository.GetByEmail(email);
        if (await _userRepository.RemoveEntity(user))
            return Ok();
        throw new Exception("Error while trying to delete user");
    }
    #endregion 


}