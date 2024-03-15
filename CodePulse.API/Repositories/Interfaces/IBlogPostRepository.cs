using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interfaces
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetAllAsync();

        Task<BlogPost?> GetBlogPostById(Guid id);

        Task<BlogPost?> GetBlogPostByurlHandle(string urlHandle);
        Task<BlogPost?> updateAsync(BlogPost blogpost);

        Task<BlogPost?> DeleteAsync(Guid id);
    }
}
