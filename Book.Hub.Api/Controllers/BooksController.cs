using Books.Hub.Application.DTOs.Books;
using Books.Hub.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Books.Hub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IOptions<ImagesOptions> _options;
        public BooksController(IBookService bookService, IOptions<ImagesOptions> options)
        {
            _bookService = bookService;
            _options = options;
        }



        /// <summary>
        /// Search Books by Name with pagination 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="search">max 100 char</param>
        /// <param name="pageNumber">Range from 1</param>
        /// <param name="pageSize">Range from 1 to 100</param>
        /// <param name="sort">select one "best-seller/top-rated/name/price/recentley-added"</param>
        /// <param name="isDesc"></param>
        /// <param name="categoryId"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <response code="200">Returns the requested paged list of Books</response>
        /// <response code="404">If no Book matching the search criteria are found.</response>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync(
            CancellationToken token,
            [FromQuery] string? search,
            [FromQuery] int? categoryId,
            [FromQuery] double? minPrice,
            [FromQuery] double? maxPrice,
            [FromQuery][Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery][Range(1, 100)] int pageSize = 20,
            [FromQuery] string sort = "best-seller",
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
            var books = await _bookService.GetAllAsync(advancedSearch, categoryId, minPrice, maxPrice, token);
            return Ok(books);
        }



        /// <summary>
        /// Create a new Book
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAysnc([FromForm] FormBookDTO dto, CancellationToken cancellationToken)
        {
            var created = await _bookService.CreateBookAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetBookDetails),
                new { id = created.Id },
                created);
        }



        /// <summary>
        /// Get book details with related data (product detail page)
        /// </summary>
        /// <response code="200">Returns the requested Book</response>
        /// <response code="404">If no Book matching the search criteria are found.</response>
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetBookDetails(int id, CancellationToken token)
        {
            var book = await _bookService.GetByIdAsync(id, token);
            return Ok(book);
        }


       
        /// <summary>
        /// Edit book with Id
        /// </summary>
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditAsync([FromRoute]int id, [FromForm] FormBookDTO dto, CancellationToken token)
        {
            var editedBook = await _bookService.EditAsync(id,dto, token);
            return Ok(editedBook);
        }



        /// <summary>
        /// Delete Book With Id
        /// </summary>
        [HttpDelete("delete/{id}")]
        //[Authorize(Roles = nameof(Roles.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeletetAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            await _bookService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
