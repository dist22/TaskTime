using TaskManager.Models;

namespace TaskManager.Interfaces;

public interface ICategoryRepository : IBaseRepository<Category>
{
    public Task<Category?> FindCategoriesByName(string categoriesName);

    public Task<Category?> CreateCategories(string categoriesName);
    
    public Task<IEnumerable<Category>> GetAllCategories();

    public Task<Category> FindCategoriesById(int categoriesId);
    
}