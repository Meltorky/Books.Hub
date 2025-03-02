using Books.Hub.Application.Comman;
using Books.Hub.Application.DTOs.Auth;
using Books.Hub.Application.Interfaces.IServices.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
