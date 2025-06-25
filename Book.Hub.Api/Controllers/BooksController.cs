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
        public async Task<IActionResult> GetAllAsync() 
        {
            return Ok(await _bookService.GetAllAsync());
        }

        [HttpGet("{id}", Name = "GetBookByIdAsync")]
        public async Task<IActionResult> GetBookByIdAsync([FromRoute] int id) 
        {
            var book = await _bookService.GetByIdAsync(id);
            return book is null ? 
                NotFound($"No Book Exist With ID: {id}") :
                Ok(book);
        }


        [HttpPost("add")]
        public async Task<IActionResult> CreateAsync([FromForm] CreateBookDTO dto) 
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.BookCoverFile is not null && !ImagesValidator.UploadedImagesValidator(dto.BookCoverFile!, _options.Value))
            {
                return BadRequest($"Only Accept |{_options.Value.AllowedExtentions.Replace(',', '|')}| with {_options.Value.MaxSizeAllowedInBytes / 1024 / 1024} MB");
            }
            var result = await _bookService.CreateBookAsync(dto);

            return result is null ? 
                BadRequest("Book Does Not Added !!") :
                Ok(result);
        }


        [HttpPost("edit")]
        public async Task<IActionResult> EditAsync(EditBookDTO dto) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.BookCoverFile is not null && !ImagesValidator.UploadedImagesValidator(dto.BookCoverFile!, _options.Value))
            {
                return BadRequest($"Only Accept |{_options.Value.AllowedExtentions.Replace(',', '|')}| with {_options.Value.MaxSizeAllowedInBytes / 1024 / 1024} MB");
            }

            var editedBook = await _bookService.EditAsync(dto);

            return editedBook is null ?
                NotFound($"No Book Exist with ID: {dto.Id} !!!") :
                Ok(editedBook);
        }

        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> DeletetAsync([FromRoute] int Id) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await _bookService.DeleteAsync(Id) ?
                Ok("Book Succssfully Deleted !!") :
                NotFound($"No Book Exist with ID: {Id} !!");
        }

    }
}
