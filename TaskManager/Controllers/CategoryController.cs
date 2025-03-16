using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Dtos;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Controller;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    #region GET()
    
    //TODO : Вивід всіх категорій✅ 1.GET()
    [HttpGet("GetAll")]
    public async Task<IEnumerable<Category>> GetAll()
    {
        return await _categoryRepository.GetAllCategories();
    }

    //TODO : Пошук категорії за її id✅ 2.GET()
    [HttpGet("GetOneById/{categoriesId}")]
    public async Task<Category> GetOneById(int categoriesId)
    {
        return await _categoryRepository.FindCategoriesById(categoriesId);
    }

    //TODO : Пошук категорії за її назвою✅ 3.GET()
    [HttpGet("GetOneByName/{categoriesName}")]
    public async Task<Category> GetOneByName(string categoriesName)
    {
        return await _categoryRepository.FindCategoriesByName(categoriesName) ??
               throw new Exception("Failed");
    }
    #endregion

    #region POST()
    
    // TODO : Додавання категорії✅ 4.POST()
    [HttpPost("Add")]
    public async Task<IActionResult> Add(string CategoriesName)
    {
        Category? categories = await _categoryRepository.FindCategoriesByName(CategoriesName);
        if (categories == null)
        {
            await _categoryRepository.CreateCategories(CategoriesName);
            return Ok();
        }
        throw new Exception("Already exists");
    }
    #endregion

    #region PUT()
    //TODO : Редагування категорії✅ 5.PUT()
    [HttpPut("Edit")]
    public async Task<IActionResult> EditById(CategoryForEditDto categoryForEditDto)
    {
        Category categories = await _categoryRepository.FindCategoriesById(categoryForEditDto.CategoriesId);
        _mapper.Map(categoryForEditDto, categories);
        if (await _categoryRepository.Update(categories))
            return Ok();
        throw new Exception("Failed to save changes");
    }
    #endregion

    #region DELETE()
       
    //TODO : Видалення категорії✅ 6.DELETE()
    [HttpDelete("DeleteCategory/{categoryId}")]
    public async Task<IActionResult> Delete(int categoryId)
    {
        Category category = await _categoryRepository.FindCategoriesById(categoryId);
        if (await _categoryRepository.RemoveEntity(category))
            return Ok();
        throw new Exception("Fail");
    }
    
    #endregion
}