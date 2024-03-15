using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        public BlogPostRepository(ApplicationDbContext dbcontext)
        {
            Dbcontext = dbcontext;
        }

        public ApplicationDbContext Dbcontext { get; }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await Dbcontext.BlogPosts.AddAsync(blogPost);
            await Dbcontext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingpost= await Dbcontext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if (existingpost != null)
            {
                Dbcontext.BlogPosts.Remove(existingpost);
                await Dbcontext.SaveChangesAsync();
                return existingpost;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
           return await Dbcontext.BlogPosts.Include(x=>x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetBlogPostById(Guid id)
        {
           return await Dbcontext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id ==id);
           
        }

        public async Task<BlogPost?> GetBlogPostByurlHandle(string urlHandle)
        {
            return await Dbcontext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> updateAsync(BlogPost blogpost)
        {
           var existingPost= await Dbcontext.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x => x.Id == blogpost.Id);
           
            if(existingPost == null)
            {
                return null;
            }
            Dbcontext.Entry(existingPost).CurrentValues.SetValues(blogpost);
            //update categories

            existingPost.Categories = blogpost.Categories;
            await Dbcontext.SaveChangesAsync();
            return blogpost;
        }
    }
}
