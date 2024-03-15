using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        public ImageRepository(IWebHostEnvironment webEnvironment,IHttpContextAccessor httpContextAccessor,ApplicationDbContext DBcontext)
        {
            WebEnvironment = webEnvironment;
            HttpContextAccessor = httpContextAccessor;
            this.DBcontext = DBcontext;
        }

        public IWebHostEnvironment WebEnvironment { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public ApplicationDbContext DBcontext { get; }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
            var images=await DBcontext.BlogImages.ToListAsync();
            return images;
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage image)
        {
            var localpath = Path.Combine(WebEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");
            using var stream = new FileStream(localpath, FileMode.Create);
            await file.CopyToAsync(stream);
            var httprequest = HttpContextAccessor.HttpContext.Request;
            var urlPath = $"{httprequest.Scheme}://{httprequest.Host}{httprequest.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.Url = urlPath;
           await DBcontext.BlogImages.AddAsync(image);
            await DBcontext.SaveChangesAsync();
            return image;
        }
    }
}
