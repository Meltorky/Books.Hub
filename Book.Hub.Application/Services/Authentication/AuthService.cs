using System.Security.Cryptography;
using Books.Hub.Application.DTOs.Auth;
using Books.Hub.Application.Identity;
using Books.Hub.Application.Interfaces.IServices.Authentication;
using Books.Hub.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Books.Hub.Application.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<RegisterResultDTO> RegisterAsync(RegisterDTO dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
            {
                return new RegisterResultDTO
                {
                    Message = "Email is already Registed !!!"
                };
            }

            if (!await _roleManager.RoleExistsAsync(dto.RoleName))
            {
                return new RegisterResultDTO
                {
                    Message = "No Role Name Exist With This Name !!!"
                };
            }

            var registerResultDTO = new RegisterResultDTO();

            var newUser = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"| {error.Description} |";
                }
                registerResultDTO.Message = errors;
                return registerResultDTO;
            }

            registerResultDTO.Email = dto.Email;
            registerResultDTO.FirstName = dto.FirstName;
            registerResultDTO.LastName = dto.LastName;

            var addToRoleAsync = await _userManager.AddToRoleAsync(newUser, dto.RoleName);

            if (addToRoleAsync.Succeeded)
            {
                registerResultDTO.RoleName = dto.RoleName;
                registerResultDTO.IsAsginedToRole = true;
            }

            return await _tokenService.CreateTokenAsync(newUser, registerResultDTO);
        }


        public async Task<RegisterResultDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return new RegisterResultDTO() { Message = "Enter a Valid Email Or Password !!" };
            }

            var registerResultDTO = new RegisterResultDTO() 
            {
                Email = dto.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleName = _userManager.GetRolesAsync(user).Result.FirstOrDefault()!,
                IsAsginedToRole = true,               
            };

            var result = await _tokenService.CreateTokenAsync(user, registerResultDTO);

            return result;
        }

        public async Task<RegisterResultDTO> RefreshTokenAsyncHandler(string token)
        {

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens!.Any(t => t.Token == token));

            if (user == null)
            {
                return new RegisterResultDTO
                {
                    Message = "Invalid token"
                };
            }

            var refreshToken = user.RefreshTokens!.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                return new RegisterResultDTO
                {
                    Message = "InActive token"
                };
            }

            refreshToken.RevokedOn = DateTime.UtcNow;


            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens!.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            return await _tokenService.CreateTokenAsync(user , new RegisterResultDTO());
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens!.Any(t => t.Token == token));

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens!.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return true;
        }


        private static RefreshToken GenerateRefreshToken()
        {
            const int TokenByteSize = 32; // Size of the random token
            const int TokenExpiryDays = 10; // Refresh token validity

            // Generate a secure random number for the token
            byte[] randomNumber = new byte[TokenByteSize];
            RandomNumberGenerator.Fill(randomNumber);

            // Create and return the RefreshToken object
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(TokenExpiryDays),
                CreatedOn = DateTime.UtcNow
            };
        }

    }
}
