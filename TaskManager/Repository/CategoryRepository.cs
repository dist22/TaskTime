using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Repository;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(DataContextEF entity) : base(entity)
    { }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        return await _entity
            .ToListAsync();
    }

    public async Task<Category?> FindCategoriesByName(string categoriesName)
    {
        return await _entity
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CategoriesName == categoriesName);
    }

    public async Task<Category> FindCategoriesById(int categoriesId)
    {
        var category = await _entity
                           .AsNoTracking()
                           .FirstOrDefaultAsync(c => c.CategoriesId == categoriesId) ??
                       throw new Exception("Category with this ID was not found.");
        return category;
    }

    public async Task<Category?> CreateCategories(string categoriesName)
    {
        Category? category = new Category
        {
            CategoriesName = categoriesName
        };
        
        await _entity.AddAsync(category);
        if (await SaveChanges())
            return category;
        throw new Exception("Error while trying to save category");
    }
}