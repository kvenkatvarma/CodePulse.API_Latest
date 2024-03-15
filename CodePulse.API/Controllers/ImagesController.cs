using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        public ImagesController(IImageRepository repo)
        {
            Repo = repo;
        }

        public IImageRepository Repo { get; }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now
                };
                await Repo.Upload(file, blogImage);
                var response = new ImageDto
                {
                    Id= blogImage.Id,
                    Title=blogImage.Title,
                    DateCreated=blogImage.DateCreated,
                    FileExtension=blogImage.FileExtension,
                    FileName=blogImage.FileName,
                    Url=blogImage.Url
                };
                return Ok(response);
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
           var blogImages= await Repo.GetAll();
            var response = new List<ImageDto>();
            foreach(var blogImage in blogImages){
                response.Add(new ImageDto
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Url = blogImage.Url
                });
            }
            return Ok(response);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName.ToLower())))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }
            if(file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File Size cannot be more than 10MB");
            }

        }
    }
}
