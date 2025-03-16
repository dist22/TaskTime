using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Repository;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(DataContextEF entity) : base(entity)
    {
    }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        return await _entity
            .ToListAsync();
    }

    public async Task<Category?> FindCategoriesByName(string CategoriesName)
    {
        return await _entity
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CategoriesName == CategoriesName);
    }

    public async Task<Category> FindCategoriesById(int categoriesId)
    {
        var category = await _entity
                           .AsNoTracking()
                           .FirstOrDefaultAsync(c => c.CategoriesId == categoriesId) ??
                       throw new Exception("Not found");
        return category;
    }

    public async Task<Category?> CreateCategories(string CategoriesName)
    {
        Category? category = new Category
        {
            CategoriesName = CategoriesName
        };
        
        await _entity.AddAsync(category);
        if (await SaveChanges())
            return category;
        throw new Exception("Error");
    }
}