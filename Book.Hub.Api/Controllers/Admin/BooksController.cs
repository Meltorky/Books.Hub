using Books.Hub.Api.Validators;
using Books.Hub.Application.DTOs.Admin;
using Books.Hub.Application.Interfaces.IServices.Admin;
using Books.Hub.Application.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Books.Hub.Api.Controllers.Admin
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

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateBookDTO dto) 
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.BookCoverFile is not null && !ImagesValidator.UploadedImagesValidator(dto.BookCoverFile!, _options.Value))
            {
                return BadRequest($"Only Accept |{_options.Value.AllowedExtentions.Replace(',', '|')}| with {_options.Value.MaxSizeAllowedInBytes / 1024 / 1024} MB");
            }
            var result = await _bookService.CreateBookAsync(dto);

            return result is null ? BadRequest("Book Does Not Added !!") : Ok(result);
        }
    }
}
