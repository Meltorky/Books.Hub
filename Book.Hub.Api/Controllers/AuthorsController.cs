using Books.Hub.Api.Validators;
using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Application.Interfaces.IService.Admin;
using Books.Hub.Application.Interfaces.IServices.Admin;
using Books.Hub.Application.Options;
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


        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var authors = await _authorService.GetAllAsync();

            if (authors is null || !authors.Any())
                return NotFound("No Authors Found.");

            return Ok(authors);
        }

        [HttpGet("{id}", Name = "GetAuthorByIdAsync")]
        public async Task<IActionResult> GetAuthorByIdAsync([FromRoute] int id)
        {
            var author = await _authorService.GetByIdAsync(id);

            if (author == null)
                return NotFound("Author not found");

            return Ok(author);
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddAuthorAsync([FromForm] CreateAuthorDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.AuthorImage is not null && !ImagesValidator.UploadedImagesValidator(dto.AuthorImageFile!, _options.Value))
            {
                return BadRequest($"Only Accept |{_options.Value.AllowedExtentions.Replace(',', '|')}| with {_options.Value.MaxSizeAllowedInBytes / 1024 / 1024} MB");
            }

            var addedAuthor = await _authorService.AddAsync(dto);

            if (addedAuthor == null)
                return BadRequest("Author Not Added !!");

            return CreatedAtRoute(nameof(GetAuthorByIdAsync), new { id = addedAuthor.Id }, addedAuthor);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditAuthorAsync([FromForm] EditAuthorDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.AuthorImageFile is not null && !ImagesValidator.UploadedImagesValidator(dto.AuthorImageFile!, _options.Value))
            {
                return BadRequest($"Only Accept {_options.Value.AllowedExtentions.Replace(',', ' ')} with {_options.Value.MaxSizeAllowedInBytes / 1024 / 1024} MB");
            }

            var updatedAuthor = await _authorService.EditAsync(dto);

            if (updatedAuthor == null)
                return BadRequest("Fail To Edit Author !!");

            return Ok(updatedAuthor);
        }


        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> DeleteAsync(int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try 
            {
                if(await _authorService.DeleteAsync(Id))
                    return Ok("Author Has Been Deleted Successfully !!");

                return NotFound("No Author Exist With This ID !!");
            }
            catch (Exception ex) 
            {
                return StatusCode(500 ,$"You Have To Delete All Books Related To This Author !!,{ex.Message}");
            }

        }

    }
}
