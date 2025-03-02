using Books.Hub.Application.DTOs.Admin;
using Books.Hub.Application.Interfaces.IServices.Admin;
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

        [HttpGet("get/{Id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int Id) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoryService.GetByIdAsync(Id);

            return category is null ?
                NotFound("No Category Exist With This ID !!") :
                Ok(category);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _categoryService.GetAllAsync());       
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromQuery] CreateCategoryDTO dto) 
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var newCategory = await _categoryService.CreateAsync(dto);
            if (newCategory == null)
                return BadRequest("Internal Server Error !!!");

            return Ok(newCategory);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditAsync([FromQuery] CategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoryService.EditAsync(dto);

            return category is null ? 
                NotFound("No Category Exist With This ID !!") : 
                Ok(category);
        }

        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> DeleteAsync(int Id) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (await _categoryService.DeleteAsync(Id))
                    return Ok("Category Has Been Deleted Successfully !!");

                return NotFound("No Category Exist With This ID !!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"You Have To Delete All Books Related To This Category !!,{ex.Message}");
            }
        } 
        
    }
}
