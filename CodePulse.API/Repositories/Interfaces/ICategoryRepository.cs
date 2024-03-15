using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category?> GetById(Guid id);
        Task<Category?> UpdateCategoryasync(Category category);
        Task<Category?> DeleteCategory(Guid id);
    }
}
