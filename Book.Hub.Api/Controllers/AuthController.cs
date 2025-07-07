using Books.Hub.Application.DTOs.Auth;
using Books.Hub.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Books.Hub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterDTO register) 
        {
            if (!ModelState.IsValid || register.RoleName == Roles.Admin.ToString())
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(register);

            if (result.IsAuthenticated) 
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(login);

            if (result.IsAuthenticated)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }
    }
}
