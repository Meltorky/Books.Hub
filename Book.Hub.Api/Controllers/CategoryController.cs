using Books.Hub.Application.DTOs.Categories;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Books.Hub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }



        /// <summary>
        /// Get all Categories sorted by name.
        /// </summary>   
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync(CancellationToken token, [FromQuery] bool desc = false)
        {
            var spec = new QuerySpecification<Category>() 
            { 
                OrderByDescending  = desc,
                OrderBy = x => x.Name
            };

            var categories = await _categoryService.GetAllAsync(spec , token);
            return Ok(categories);
        }



        /// <summary>
        /// search all Categories by name.
        /// </summary>   
        [HttpGet("search")]
        public async Task<IActionResult> GetAllAsync(CancellationToken token, [FromQuery] string search)
        {
            var spec = new QuerySpecification<Category>();
            spec.AddCriteria(a => a.Name.Contains(search));

            var categories = await _categoryService.GetAllAsync(spec , token);
            return Ok(categories);
        }



        /// <summary>
        /// Get popular Categories which includs bigger nummber of books.
        /// </summary>   
        [HttpGet("popular")]
        public async Task<IActionResult> GetAllAsync(CancellationToken token)
        {
            var spec = new QuerySpecification<Category>();
            spec.OrderBy = x => x.BookCategories.Count;
            spec.OrderByDescending = true;

            var categories = await _categoryService.GetAllAsync(spec, token);
            return Ok(categories);
        }



        /// <summary>
        /// Get Category By Id (for Admin)
        /// </summary>
        [HttpGet("get/{Id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int Id , CancellationToken token) 
        {
            var category = await _categoryService.GetByIdAsync(Id , token);
            return Ok(category);
        }



        /// <summary>
        /// Create a new category (by admin)
        /// </summary>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync([FromQuery] CreateCategoryDTO dto , CancellationToken token) 
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            // Service throws ArgumentException or others on validation errors -> Global middleware handles them
            var newCategory = await _categoryService.CreateAsync(dto , token);

            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = newCategory.Id} ,
                newCategory);
        }



        /// <summary>
        /// edit category (by admin)
        /// </summary>
        [HttpPost("edit")]
        public async Task<IActionResult> EditAsync([FromQuery] CategoryDTO dto , CancellationToken token)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoryService.EditAsync(dto , token);
            return Ok(category);
        }



        /// <summary>
        /// Delete category (by admin)
        /// </summary>
        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> DeleteAsync(int Id , CancellationToken token) 
        {
            await _categoryService.DeleteAsync(Id ,token);
            return NoContent();
        } 
        
    }
}
