using Books.Hub.Api.Validators;
using Books.Hub.Application.DTOs.Books;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Application.Options;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;
using Books.Hub.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        /// Get all books with full details (admin dashboard)
        /// </summary>
        [HttpGet("full-details")]
        public async Task<IActionResult> GetFullDetails(CancellationToken token)
        {
            var spec = new QuerySpecification<Book>();

            spec.AddInclude(q => q
                .Include(b => b.Author));

            spec.AddInclude(q => q
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category));

            var books = await _bookService.GetAllAsync(spec, token);
            return Ok(books);
        }



        /// <summary>
        /// Search books by title with pagination (catalog page)
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks(
            CancellationToken token,
            [FromQuery] string query,
            [FromQuery][Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")] int page = 1,
            [FromQuery][Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")] int pageSize = 10)
        {
            var spec = new QuerySpecification<Book>();
            spec.AddCriteria(b => b.Name.Contains(query));
            //spec.Skip = (page - 1) * pageSize;
            //spec.Take = pageSize;
            spec.OrderBy = b => b.Name;

            var books = await _bookService.GetAllAsync(spec, token);
            return Ok(books);
        }



        /// <summary>
        /// Get all books by category (sort: name/date/price) (category page) 
        /// </summary>
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(
            int categoryId,
            CancellationToken token,
            [FromQuery] string sort = "name",
            [FromQuery] bool desc = false)
        {
            var spec = new QuerySpecification<Book>();
            spec.AddCriteria(b => b.BookCategories.Any(bc => bc.CategoryId == categoryId));
            spec.AddInclude(b => b.Include(a => a.Author));
            spec.AddInclude(b => b.Include(a => a.BookCategories).ThenInclude(c => c.Category));

            // Dynamic sorting
            spec.OrderBy = sort switch
            {
                "price" => b => b.Price,
                "date" => b => b.PublishedDate,
                _ => b => b.Name
            };
            spec.OrderByDescending = desc;

            var books = await _bookService.GetAllAsync(spec, token);
            return Ok(books);
        }



        /// <summary>
        /// Scenario 4: Get featured books (home page)
        /// </summary>
        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedBooks(CancellationToken token)
        {
            var spec = new QuerySpecification<Book>();
            spec.AddCriteria(b => b.TotalCopiesSold > 0);
            //spec.Take = 10;
            spec.OrderBy = b => b.Rating;
            spec.OrderByDescending = true;
            spec.AddInclude(b => b.Include(a => a.Author));
            spec.AddInclude(b => b.Include(a => a.BookCategories).ThenInclude(c => c.Category));

            var books = await _bookService.GetAllAsync(spec, token);
            return Ok(books);
        }



        /// <summary>
        /// Get book details with related data (product detail page)
        /// </summary>
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetBookDetails(int id, CancellationToken token)
        {
            var spec = new QuerySpecification<Book>();

            spec.AddInclude(q => q
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            );

            spec.AddInclude(q => q
                .Include(b => b.Author)
            );


            var book = await _bookService.GetByIdAsync(id, spec, token);
            return book != null ? Ok(book) : NotFound();
        }



        /// <summary>
        /// Get books by multiple filters (advanced search)
        /// </summary>
        [HttpGet("advanced")]
        public async Task<IActionResult> AdvancedSearch(
            CancellationToken token,
            [FromQuery] string? name,
            [FromQuery] double? minPrice,
            [FromQuery] double? maxPrice,
            [FromQuery][Range(1, 10, ErrorMessage = "Page must be between 1 - 10")] double? minRating,
            [FromQuery] string? sort = "name",
            [FromQuery] bool? desc = true)
        {
            var spec = new QuerySpecification<Book>();

            if (!string.IsNullOrEmpty(name))
                spec.AddCriteria(b => b.Name.Contains(name));

            if (minPrice.HasValue)
                spec.AddCriteria(b => b.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                spec.AddCriteria(b => b.Price <= maxPrice.Value);

            if (minRating.HasValue)
                spec.AddCriteria(b => b.Rating >= minRating.Value);


            spec.AddInclude(b => b.Include(a => a.Author));
            spec.OrderByDescending = desc ?? false;
            spec.OrderBy = sort switch
            {
                "price" => b => b.Price,
                "date" => b => b.PublishedDate,
                _ => b => b.Name
            };

            var books = await _bookService.GetAllAsync(spec, token);
            return Ok(books);
        }



        /// <summary>
        /// Returns Book without includes
        /// </summary>
        /// <response code="200">Returns Book without includes.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookByIdAsync([FromRoute] int id, CancellationToken cancellationToken) 
        {
            return Ok(await _bookService.GetByIdAsync(id, null ,cancellationToken));
        }



        /// <summary>
        /// create book
        /// </summary>
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuthorProfile([FromForm] CreateBookDTO dto , CancellationToken cancellationToken) 
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate the uploaded image
            if (dto.BookCoverFile is not null && !ImagesValidator.UploadedImagesValidator(dto.BookCoverFile!, _options.Value))
            {
                return BadRequest($"Only Accept |{_options.Value.AllowedExtentions.Replace(',', '|')}| with {_options.Value.MaxSizeAllowedInBytes / 1024 / 1024} MB");
            }

            // Service throws ArgumentException or others on validation errors -> Global middleware handles them
            var created = await _bookService.CreateBookAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetBookByIdAsync),      // action
                new { id = created.Id },       // route values
                created);                      // response body
        }



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
