using TaskManager.Models;

namespace TaskManager.Interfaces;

public interface ICategoryRepository : IBaseRepository<Category>
{
    public Task<Category?> FindCategoriesByName(string CategoriesName);

    public Task<Category?> CreateCategories(string CategoriesName);
    
    public Task<IEnumerable<Category>> GetAllCategories();

    public Task<Category> FindCategoriesById(int categoriesId);
    

}