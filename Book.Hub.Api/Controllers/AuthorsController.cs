using System.ComponentModel.DataAnnotations;
using Books.Hub.Api.Validators;
using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Application.Options;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Books.Hub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly IOptions<ImagesOptions> _options;
        public AuthorsController(IAuthorService authorService, IOptions<ImagesOptions> options)
        {
            _authorService = authorService;
            _options = options;
        }



        /// <summary>
        /// Get all Authors with full details (Order By: popular/name)(admin dashboard)
        /// </summary>
        [HttpGet("full-details")]  
        public async Task<IActionResult> GetAllAsync(
            CancellationToken token,
            [FromQuery] string sort = "popular",
            [FromQuery] bool desc = true)
        {
            var spec = new QuerySpecification<Author>();
            spec.OrderByDescending = desc;
            spec.OrderBy = sort switch 
            {
                "popular" => a => a.Books.Count,
                _ => a => a.Name,
            };

            var authors = await _authorService.GetAllAsync(spec,token);
            return Ok(authors);
        }



        /// <summary>
        /// Search Authors by Name with pagination (Order By: trending/popular/name) (catalog page)
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> GetAllAsync(
            CancellationToken token,
            [FromQuery] string search,
            [FromQuery][Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")] int page = 1,
            [FromQuery][Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")] int pageSize = 10,
            [FromQuery] string sort = "trending",
            [FromQuery] bool desc = true)
        {
            var spec = new QuerySpecification<Author>();
            spec.AddCriteria(a => a.Name.Contains(search));
            spec.Skip = (page-1) * pageSize;
            spec.Take = pageSize;
            spec.OrderByDescending = desc;
            spec.OrderBy = sort switch
            {
                "trending" => a => a.AuthorSubscribers.Count,
                "popular" => a => a.Books.Count,
                _ => a => a.Name,
            };

            var authors = await _authorService.GetAllAsync(spec, token);
            return Ok(authors);
        }



        /// <summary>
        /// Get specific Author with full related books (Author Manager)
        /// </summary>
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken token)
        {
            var spec = new QuerySpecification<Author>();
            spec.AddInclude(q => q.Include(a => a.Books));

            var author = await _authorService.GetByIdAsync(id,spec,token);
            return Ok(author);
        }



        /// <summary>
        /// Get specific Author details simple (admin dashboard)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> SimpleGetByIdAsync([FromRoute] int id, CancellationToken token)
        {
            var author = await _authorService.GetByIdAsync(id, null,token);
            return Ok(author);
        }





        /// <summary>
        /// Create Author profile (by Admin)
        /// </summary>
        [HttpPost("admin/create-profile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuthorProfile([FromForm] CreateAuthorDTO dto, CancellationToken token)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate the uploaded image
            if (dto.AuthorImage is not null && !ImagesValidator.UploadedImagesValidator(dto.AuthorImageFile!, _options.Value))
            {
                return BadRequest($"Only Accept |{_options.Value.AllowedExtentions.Replace(',', '|')}| with {_options.Value.MaxSizeAllowedInBytes / 1024 / 1024} MB");
            }

            // Service throws ArgumentException or others on validation errors -> Global middleware handles them
            var addedAuthor = await _authorService.CreateAuthorProfile(dto, token);
           
            return CreatedAtRoute(
                nameof(GetByIdAsync), 
                new { id = addedAuthor.Id }, 
                addedAuthor);
        }



        /// <summary>
        /// Create Author profile (by Author)
        /// </summary>
        /// <param name="id">The unique identifier for the Author User Account.</param>
        /// <param name="dto">The data transfer object containing the details for creating the author profile.</param>
        /// <param name="token">The cancellation token to cancel the operation.</param>
        [HttpPost("author/create-profile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuthorProfile(
            [FromRoute] string id,
            [FromForm] CreateAuthorDTO dto,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate the uploaded image
            if (dto.AuthorImage is not null && !ImagesValidator.UploadedImagesValidator(dto.AuthorImageFile!, _options.Value))
            {
                return BadRequest($"Only Accept |{_options.Value.AllowedExtentions.Replace(',', '|')}| with {_options.Value.MaxSizeAllowedInBytes / 1024 / 1024} MB");
            }

            // Service throws ArgumentException or others on validation errors -> Global middleware handles them
            var addedAuthor = await _authorService.CreateAuthorProfile(id,dto,token);

            return CreatedAtRoute(
                nameof(GetByIdAsync),
                new { id = addedAuthor.Id },
                addedAuthor);
        }



        /// <summary>
        /// edit Author Profile by Admin/Author
        /// </summary>
        [HttpPost("edit")]
        public async Task<IActionResult> EditAuthorAsync([FromForm] EditAuthorDTO dto , CancellationToken token)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.AuthorImageFile is not null && !ImagesValidator.UploadedImagesValidator(dto.AuthorImageFile!, _options.Value))
            {
                return BadRequest($"Only Accept {_options.Value.AllowedExtentions.Replace(',', ' ')} with {_options.Value.MaxSizeAllowedInBytes / 1024 / 1024} MB");
            }

            var updatedAuthor = await _authorService.EditAsync(dto, token);
            return Ok(updatedAuthor);
        }



        /// <summary>
        /// Delete AuthorProfile by Addmin/Author
        /// </summary>
        [HttpDelete("delete/{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int Id , CancellationToken token)
        {
            await _authorService.DeleteAsync(Id, token);
            return NoContent();
        }

    }
}
    