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
    
    //TODO : Display all categories 1.GET()
    [HttpGet("GetAll")]
    public async Task<IEnumerable<Category>> GetAll()
    {
        return await _categoryRepository.GetAllCategories();
    }

    //TODO : Search for a category by its id 2.GET()
    [HttpGet("GetOneById/{categoriesId}")]
    public async Task<Category> GetOneById(int categoriesId)
    {
        return await _categoryRepository.FindCategoriesById(categoriesId);
    }

    //TODO : Search for a category by its name 3.GET()
    [HttpGet("GetOneByName/{categoriesName}")]
    public async Task<Category> GetOneByName(string categoriesName)
    {
        return await _categoryRepository.FindCategoriesByName(categoriesName) ??
               throw new Exception("A category with this name was not found");
    }
    #endregion

    #region POST()
    
    // TODO : Adding a category 4.POST()
    [HttpPost("Add")]
    public async Task<IActionResult> Add(string categoriesName)
    {
        Category? categories = await _categoryRepository.FindCategoriesByName(categoriesName);
        if (categories == null)
        {
            await _categoryRepository.CreateCategories(categoriesName);
            return Ok();
        }
        throw new Exception("This category already exists");
    }
    #endregion

    #region PUT()
    //TODO : Editing a category 5.PUT()
    [HttpPut("Edit")]
    public async Task<IActionResult> EditById(CategoryForEditDto categoryForEditDto)
    {
        Category categories = await _categoryRepository.FindCategoriesById(categoryForEditDto.CategoriesId);
        _mapper.Map(categoryForEditDto, categories);
        if (await _categoryRepository.Update(categories))
            return Ok();
        throw new Exception("Error while trying to update category");
    }
    #endregion

    #region DELETE()
       
    //TODO : Delete a category 6.DELETE()
    [HttpDelete("DeleteCategory/{categoryId}")]
    public async Task<IActionResult> Delete(int categoryId)
    {
        Category category = await _categoryRepository.FindCategoriesById(categoryId);
        if (await _categoryRepository.RemoveEntity(category))
            return Ok();
        throw new Exception("Error while trying to delete category");
    }
    
    #endregion
}