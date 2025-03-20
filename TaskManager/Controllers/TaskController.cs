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
public class TaskController : ControllerBase
{
    private readonly ITasksRepository _tasksRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public TaskController(ITasksRepository tasksRepository, IMapper mapper, IUserRepository userRepository,
        ICategoryRepository categoryRepository)
    {
        _tasksRepository = tasksRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    #region GET()

    //Get all tasks 1.GET()
    [HttpGet("GetAll")]
    public async Task<IEnumerable<TaskDto>> GetAll()
    {
        var taskDto = await _tasksRepository.GetAllTasks();
        var task = _mapper.Map<IEnumerable<TaskDto>>(taskDto);
        return task;
    }

    //Get task by id 2.GET()
    [HttpGet("GetTaskById/{taskId}")]
    public async Task<TaskDto> GetTaskById(int taskId)
    {
        var taskDto = await _tasksRepository.GetById(taskId);
        var task = _mapper.Map<TaskDto>(taskDto);
        return task;
    }

    //Get all active tasks 3.GET()
    [HttpGet("GetActive")]
    public async Task<IEnumerable<TaskDto>> GetActive()
    {
        var taskDto = await _tasksRepository.GetActive();
        var task = _mapper.Map<IEnumerable<TaskDto>>(taskDto);
        return task;
    }

    //Get all tasks by category 4.GET()
    [HttpGet("GetByCategory/{categoryName}")]
    public async Task<IEnumerable<TaskDto>> GetByCategory(string categoryName)
    {
        var taskDto = await _tasksRepository.GetByCategory(categoryName);
        var task = _mapper.Map<IEnumerable<TaskDto>>(taskDto);
        return task;
    }

    #endregion

    #region POST()

    //Create a task 5.POST()
    [HttpPost("CreateTask")]
    public async Task<IActionResult> Create(CreateTaskDto createTaskDto)
    {
        var category = await _categoryRepository.FindCategoriesByName(createTaskDto.CategoryName)
                       ?? await _categoryRepository.CreateCategories(createTaskDto.CategoryName);
        var task = _mapper.Map<TaskTime>(createTaskDto);
        await _tasksRepository.Add(task, category);
        return Ok();
    }

    #endregion POST()

    #region PUT()

    //Edit task 6.PUT()
    [HttpPut("EditTask")]
    public async Task<IActionResult> EditTask(TaskForEdit taskForEdit)
    {
        TaskTime task = await _tasksRepository.GetById(taskForEdit.TaskId);
        _mapper.Map(taskForEdit, task);
        if (await _tasksRepository.Update(task))
            return Ok();
        throw new Exception("Error while trying to update task");
    }

    //Change task category 7.PUT()
    [HttpPut("ChangeTaskCategory/{taskId}/{categoryName}")]
    public async Task<IActionResult> ChangeCategory(int taskId, string categoryName)
    {
        TaskTime task = await _tasksRepository.GetById(taskId);
        var category = await _categoryRepository.FindCategoriesByName(categoryName)
                       ?? await _categoryRepository.CreateCategories(categoryName);
        await _tasksRepository.ChangeCategory(task, category);
        return Ok();
    }

    //Add user to task 8.PUT()
    [HttpPut("AddUser/{userId}/{taskId}")]
    public async Task<IActionResult> AddUser(int userId, int taskId)
    {
        var user = await _userRepository.GetById(userId);
        var task = await _tasksRepository.GetById(taskId);

        await _tasksRepository.AddUserToCategory(user, task);
        await _userRepository.AddTaskToUser(user, task);
        return Ok();
    }

    //Change task status 9.PUT()
    [HttpPut("ChangeStatus/{taskId}/{status}")]
    public async Task<IActionResult> ChangeStatus(int taskId, bool status)
    {
        var task = await _tasksRepository.GetById(taskId);
        task.isComplete = status;
        if (await _tasksRepository.Update(task))
            return Ok();
        throw new Exception("Error while trying to update task");
    }

    #endregion

    #region DELETE()

    //Delete task 10.DELETE()
    [HttpDelete("DeleteTask/{taskid}")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        var task = await _tasksRepository.GetById(taskId);
        if (await _tasksRepository.RemoveEntity(task))
            return Ok();
        throw new Exception("Error while trying to delete a task");
    }

    #endregion
}