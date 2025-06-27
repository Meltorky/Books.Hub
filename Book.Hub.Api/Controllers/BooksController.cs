using Books.Hub.Api.Validators;
using Books.Hub.Application.DTOs.Books;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Application.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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


        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken) 
        {
            return Ok(await _bookService.GetAllAsync(cancellationToken));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookByIdAsync([FromRoute] int id, CancellationToken cancellationToken) 
        {
            return Ok(await _bookService.GetByIdAsync(id, cancellationToken));
        }


        [HttpPost("add")]
        public async Task<IActionResult> CreateAsync([FromForm] CreateBookDTO dto , CancellationToken cancellationToken) 
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


        [HttpPost("edit")]
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


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletetAsync([FromRoute] int id, CancellationToken cancellationToken) 
        {
            await _bookService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
