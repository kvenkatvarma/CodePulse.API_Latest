using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public ApplicationDbContext DbContext { get; }

        public  async Task<Category> CreateCategoryAsync(Category category)
        {
            await DbContext.Categories.AddAsync(category);
            await DbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> DeleteCategory(Guid id)
        {
            var category = await DbContext.Categories.FirstOrDefaultAsync(c => c.Id == id); 
            if(category == null)
            {
                return null;
            }
            DbContext.Categories.Remove(category);
            await DbContext.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await DbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
            return await DbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateCategoryasync(Category category)
        {
            var exisitngctegory = await DbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if(exisitngctegory != null)
            {
                DbContext.Entry(exisitngctegory).CurrentValues.SetValues(category);
                await DbContext.SaveChangesAsync();
                return category;
            }
            return null;
        }
    }
}
