using System.ComponentModel.DataAnnotations;
using Books.Hub.Api.Validators;
using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Domain.Common;
using Microsoft.AspNetCore.Mvc;
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
        /// Search Authors by Name with pagination (catalog page) 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="search">max 100 char</param>
        /// <param name="pageNumber">Range from 1</param>
        /// <param name="pageSize">Range from 1 to 100</param>
        /// <param name="sort">select one "trending/popular/name"</param>
        /// <param name="isDesc"></param>
        /// <response code="200">Returns the requested paged list of Authors</response>
        /// <response code="404">If no Authors matching the search criteria are found</response>
        [HttpGet("All")]
        public async Task<IActionResult> GetAllAsync(
            CancellationToken token,
            [FromQuery] string? search,
            [FromQuery][Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery][Range(1, 100)] int pageSize = 20,
            [FromQuery] string sort = "trending",
            [FromQuery] bool isDesc = true)
        {
            AdvancedSearch advancedSearch = new AdvancedSearch
            {
                searchText = search,
                pageNumber = pageNumber,
                resultsPerPage = pageSize,
                SortedBy = sort,
                IsDesc = isDesc,
            };
            var authors = await _authorService.GetAllAsync(advancedSearch, token);
            return Ok(authors);
        }



        /// <summary>
        /// Get specific Author details simple without Books (admin dashboard)
        /// </summary>
        [HttpGet("{id}", Name = "SimpleGetByIdAsync")]
        public async Task<IActionResult> SimpleGetByIdAsync([FromRoute] int id, CancellationToken token)
        {
            var author = await _authorService.SimpleGetByIdAsync(id, token);
            return Ok(author);
        }
        
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserIdAsync([FromRoute] string userId, CancellationToken token)
        {
            var author = await _authorService.GetByUserIdAsync(userId, token);
            return Ok(author);
        }



        /// <summary>
        /// Get specific Author with full related books
        /// </summary>
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken token)
        {
            var author = await _authorService.GetByIdAsync(id, token);
            return Ok(author);
        }



        /// <summary>
        /// Create Author profile (by Admin)
        /// </summary>
        [HttpPost("admin/create-profile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuthorProfile([FromForm] CreateAuthorDTO dto, CancellationToken token)
        {
            var addedAuthor = await _authorService.CreateAuthorProfile(null, dto, token);

            return CreatedAtRoute(
                nameof(SimpleGetByIdAsync),
                new { id = addedAuthor.Id },
                addedAuthor
            );
        }



        /// <summary>
        /// Create Author profile (by User)
        /// </summary>
        /// <param name="id">The unique identifier for the User Account.</param>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        [HttpPost("author/create-profile/{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuthorProfile(
            [FromRoute] string id,
            [FromForm] CreateAuthorDTO dto,
            CancellationToken token)
        {
            var addedAuthor = await _authorService.CreateAuthorProfile(id, dto, token);

            return CreatedAtRoute(
                nameof(SimpleGetByIdAsync),
                new { id = addedAuthor.Id },
                addedAuthor
            );
        }



        /// <summary>
        /// Edit Author Profile by Admin/Author
        /// </summary>
        [HttpPut("edit")]
        public async Task<ActionResult<AuthorDTO>> EditAuthorAsync([FromForm] EditAuthorDTO dto, CancellationToken token)
        {
            var result = await _authorService.EditAsync(dto, token);
            return Ok(result);
        }



        /// <summary>
        /// Delete AuthorProfile by Addmin/Author
        /// </summary>
        //[Authorize(Roles = nameof(Roles.Admin))]
        [HttpDelete("delete/{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int Id, CancellationToken token)
        {
            await _authorService.DeleteAsync(Id, token);
            return NoContent();
        }
    }
}
