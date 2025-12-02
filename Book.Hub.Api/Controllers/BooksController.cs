using Books.Hub.Api.Validators;
using Books.Hub.Application.DTOs.Books;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;
using Books.Hub.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
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
            var authors = await _bookService.GetAllAsync(advancedSearch, categoryId, minPrice, maxPrice, token);
            return Ok(authors);
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


        
        // [HttpPost("add")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //public async Task<IActionResult> CreateAuthorProfile([FromForm] CreateBookDTO dto, CancellationToken cancellationToken)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    // Validate the uploaded image
        //    if (dto.BookCoverFile is not null && !ImagesValidator.UploadedImagesValidator(dto.BookCoverFile!, _options.Value))
        //    {
        //        return BadRequest($"Only Accept |{_options.Value.AllowedExtentions.Replace(',', '|')}| with {_options.Value.MaxSizeAllowedInBytes / 1024 / 1024} MB");
        //    }

        //    // Service throws ArgumentException or others on validation errors -> Global middleware handles them
        //    var created = await _bookService.CreateBookAsync(dto, cancellationToken);

        //    return CreatedAtAction(
        //        nameof(GetBookByIdAsync),      // action
        //        new { id = created.Id },       // route values
        //        created);                      // response body
        //}



        /// <summary>
        /// Edit book with Id
        /// </summary>
        [HttpPut("edit")]
        public async Task<IActionResult> EditAsync(EditBookDTO dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.BookCoverFile is not null && !ImagesValidator.UploadedImagesValidator(dto.BookCoverFile!, _options.Value))
            {
                return BadRequest($"Only Accept |{_options.Value.AllowedExtentions.Replace(',', '|')}| with {_options.Value.MaxSizeAllowedInBytes / 1024 / 1024} MB");
            }

            var editedBook = await _bookService.EditAsync(dto, cancellationToken);
            return Ok(editedBook);
        }



        /// <summary>
        /// Delete Book With Id
        /// </summary>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeletetAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            await _bookService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
