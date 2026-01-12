using System.Net.Mail;
using System.Security.Cryptography;
using Books.Hub.Application.Common.Exceptions;
using Books.Hub.Application.DTOs.Auth;
using Books.Hub.Application.Identity;
using Books.Hub.Application.Interfaces.IServices.Authentication;
using Books.Hub.Application.Mappers;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Enums;
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
                throw new DuplicateEmailException("Email is already registied !!");

            if (!await _roleManager.RoleExistsAsync(dto.RoleName) || dto.RoleName == Roles.Admin.ToString())
                throw new NotFoundException("InValid role name !!");

            var registerResultDTO = new RegisterResultDTO();

            var newUser = dto.ToAppUser();

            if (await _userManager.FindByNameAsync(newUser.UserName!) is not null)
                newUser.UserName = $"{newUser.UserName}0";

            var result = await _userManager.CreateAsync(newUser, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join('|', result.Errors.Select(x => x.Description));
                throw new OperationFailedException(errors);
            }

            registerResultDTO.bindAppUser(newUser);

            var addToRoleAsync = await _userManager.AddToRoleAsync(newUser, dto.RoleName);

            if (addToRoleAsync.Succeeded)
                registerResultDTO.RoleName = dto.RoleName;

            return await _tokenService.CreateTokenAsync(newUser, registerResultDTO);
        }


        public async Task<RegisterResultDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new NotFoundException("InValid email or password !!");

            var registerResultDTO = new RegisterResultDTO() 
            {
                Email = dto.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName!,
                RoleName = string.Join('|',await _userManager.GetRolesAsync(user)),
            };

            var result = await _tokenService.CreateTokenAsync(user, registerResultDTO);

            return result;
        }


        public async Task<RegisterResultDTO> RefreshTokenAsyncHandler(string token)
        {

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens!.Any(t => t.Token == token))
                ?? throw new NotFoundException("Invalid token !!");

            var refreshToken = user.RefreshTokens!
                .SingleOrDefault(t => t.Token == token && t.IsActive == true)
                ?? throw new OperationFailedException("InActive token !!");

            refreshToken.RevokedOn = DateTime.UtcNow;

            user.RefreshTokens!.Add(GenerateRefreshToken());
            await _userManager.UpdateAsync(user);

            var registerResultDTO = new RegisterResultDTO()
            {
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName!,
                RoleName = string.Join('|', await _userManager.GetRolesAsync(user)),
            };

            return await _tokenService.CreateTokenAsync(user, registerResultDTO);
        }


        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens!.Any(t => t.Token == token));

            if (user == null) return false;

            var refreshToken = user.RefreshTokens!
                .SingleOrDefault(t => t.Token == token && t.IsActive == true);

            if (refreshToken == null) return false;

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
