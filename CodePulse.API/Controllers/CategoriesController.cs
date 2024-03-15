using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        public CategoriesController(ICategoryRepository repo)
        {
            Repo = repo;
        }

        public ICategoryRepository Repo { get; }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            //Map the requestDTo to Domain Model as Repositories deals with Models
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
           
            await Repo.CreateCategoryAsync(category);
            //Map the Model to DTO
            var response = new CategoryDto
            {
                Id=category.Id,
                Name=category.Name,
                UrlHandle=category.UrlHandle
            };
            return Ok(response);
        }

        [HttpGet]        
        public async Task<IActionResult> GetAll()
        {
           var categories= await Repo.GetAllCategories();
            var response = new List<CategoryDto>();
            foreach(var category in categories)
            {
                response.Add(new CategoryDto
                {
                    Name = category.Name,
                    UrlHandle=category.UrlHandle,
                    Id=category.Id
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var exisitingCateory = await Repo.GetById(id);
            if(exisitingCateory == null)
            {
                return NotFound();
            }
            var response = new CategoryDto()
            {
                Id=exisitingCateory.Id,
                Name=exisitingCateory.Name,
                UrlHandle=exisitingCateory.UrlHandle
            };
            return Ok(response);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> updateCategoryAsync([FromRoute] Guid id, UpdateCategoryRequestDto request)
        {
            var category = new Category
            {
               Name=request.Name,
               Id=id,
               UrlHandle=request.UrlHandle
            };
            category=await Repo.UpdateCategoryasync(category);
            if(category == null)
            {
                return NotFound();
            }
            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(categoryDto);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await Repo.DeleteCategory(id);
            if(category == null)
            {
                return BadRequest();
            }

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(categoryDto);
        }
    }
}
