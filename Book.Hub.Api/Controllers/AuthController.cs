using Books.Hub.Application.DTOs.Auth;
using Books.Hub.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
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


        /// <summary>
        /// Register a new user (Author or Subscriber)
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterDTO register) 
        {
            if (!ModelState.IsValid || register.RoleName == Roles.Admin.ToString())
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(register);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefrehTokenInTheCookie(result.RefreshToken, result.RefreshTokenExpiresOn);

            if (result.IsAuthenticated) 
                return Ok(result);

            return BadRequest(result.Message);
        }


        /// <summary>
        /// Login
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(login);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefrehTokenInTheCookie(result.RefreshToken, result.RefreshTokenExpiresOn);

            if (result.IsAuthenticated)
                return Ok(result);

            return BadRequest(result.Message);
        }


        /// <summary>
        /// create new JWT/refresh tokens, To allows silent re-authentication without logging the user out.
        /// </summary>
        /// <response code="200">returns new access + refresh token, and stores the refresh token in cookie again.</response>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authService.RefreshTokenAsyncHandler(refreshToken ?? string.Empty);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefrehTokenInTheCookie(result.RefreshToken!, result.RefreshTokenExpiresOn);

            return Ok(result);
        }


        /// <summary>
        /// Revoke refresh token, to allows a user to log out or revoke sessions
        /// </summary>
        /// <param name="dto">The refresh token</param>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost("revokeToken")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDTO dto)
        {
            var token = dto.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _authService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is invalid!");

            return NoContent();
        }


        private void SetRefrehTokenInTheCookie(string refreshToken, DateTime expireson)
        {
            var cookieOption = new CookieOptions
            {
                HttpOnly = true,
                Expires = expireson.ToLocalTime(),
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOption);
        }

    }
}
