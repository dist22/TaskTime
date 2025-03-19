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

    //TODO : Отримати всі таски✅ 1.GET()
    [HttpGet("GetAll")]
    public async Task<IEnumerable<TaskDto>> GetAll()
    {
        var taskDto = await _tasksRepository.GetAllTasks();
        var task = _mapper.Map<IEnumerable<TaskDto>>(taskDto);
        return task;
    }

    //TODO : Отримати таску за id✅ 2.GET()
    [HttpGet("GetTaskById/{taskId}")]
    public async Task<TaskDto> GetTaskById(int taskId)
    {
        var taskDto = await _tasksRepository.GetById(taskId);
        var task = _mapper.Map<TaskDto>(taskDto);
        return task;
    }

    //TODO : Отримати всі активні таски ✅ 3.GET()
    [HttpGet("GetActive")]
    public async Task<IEnumerable<TaskDto>> GetActive()
    {
        var taskDto = await _tasksRepository.GetActive();
        var task = _mapper.Map<IEnumerable<TaskDto>>(taskDto);
        return task;
    }

    //TODO : Отримати всі таски за катерогією ✅ 4.GET()
    [HttpGet("GetByCategory/{categoryName}")]
    public async Task<IEnumerable<TaskDto>> GetByCategory(string categoryName)
    {
        var taskDto = await _tasksRepository.GetByCategory(categoryName);
        var task = _mapper.Map<IEnumerable<TaskDto>>(taskDto);
        return task;
    }

    #endregion

    #region POST()

    //TODO : Створити таску ✅ 5.POST()
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

    //TODO : Редагувати таску✅ 6.PUT()
    [HttpPut("EditTask")]
    public async Task<IActionResult> EditTask(TaskForEdit taskForEdit)
    {
        TaskTime task = await _tasksRepository.GetById(taskForEdit.TaskId);
        _mapper.Map(taskForEdit, task);
        if (await _tasksRepository.Update(task))
            return Ok();
        throw new Exception("Error");
    }

    //TODO : Змітини категоріїю таски✅ 7.PUT()
    [HttpPut("ChangeTaskCategory/{taskId}/{categoryName}")]
    public async Task<IActionResult> ChangeCategory(int taskId, string categoryName)
    {
        TaskTime task = await _tasksRepository.GetById(taskId);
        var category = await _categoryRepository.FindCategoriesByName(categoryName)
                       ?? await _categoryRepository.CreateCategories(categoryName);
        await _tasksRepository.ChangeCategory(task, category);
        return Ok();
    }

    //TODO : Дадати користувача до таски✅ 8.PUT()
    [HttpPut("AddUser/{userId}/{taskId}")]
    public async Task<IActionResult> AddUser(int userId, int taskId)
    {
        var user = await _userRepository.GetById(userId);
        var task = await _tasksRepository.GetById(taskId);

        await _tasksRepository.AddUserToCategory(user, task);
        await _userRepository.AddTaskToUser(user, task);
        return Ok();
    }

    //TODO : Змінити статус таски✅ 9.PUT()
    [HttpPut("ChangeStatus/{taskId}/{status}")]
    public async Task<IActionResult> ChangeStatus(int taskId, bool status)
    {
        var task = await _tasksRepository.GetById(taskId);
        task.isComplete = status;
        if (await _tasksRepository.Update(task))
            return Ok();
        throw new Exception("Fail");
    }

    #endregion

    #region DELETE()

    //TODO : Видалити таску✅ 10.DELETE()
    [HttpDelete("DeleteTask/{taskid}")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        var task = await _tasksRepository.GetById(taskId);
        if (await _tasksRepository.RemoveEntity(task))
            return Ok();
        throw new Exception("Error");
    }

    #endregion
}