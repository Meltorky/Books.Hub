using Books.Hub.Application.DTOs.Auth;
using Books.Hub.Application.Identity;
using Books.Hub.Application.Interfaces.IServices.Authentication;
using Books.Hub.Appliction.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Services.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtOptions jwtOptions;

        public TokenService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JwtOptions> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            jwtOptions = jwt.Value; // Extract the JwtOptions value from IOptions
        }

        public async Task<RegisterResultDTO> CreateTokenAsync(ApplicationUser user, RegisterResultDTO dto)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Create a list of standard claims for the token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName!), // User's username
                new Claim(ClaimTypes.Email, user.Email!), // Email claim (alternative format)
             }
            .Union(userClaims) // Combine with user-specific claims
            .Union(roleClaims); // Combine with role claims

            var tokenHandler = new JwtSecurityTokenHandler();

            // Define the token descriptor (configuration for the token)
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // Add all claims to the token
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)), SecurityAlgorithms.HmacSha256),
                Expires = DateTime.Now.AddDays(double.Parse(jwtOptions.LfeTimeInDays))
            };

            var createdToken = tokenHandler.CreateToken(descriptor);
            var token = tokenHandler.WriteToken(createdToken);

            // Map to RegisterResultDTO
            dto.Message = "Token created successfully !!";
            dto.IsAuthenticated = true;
            dto.ExpiresOn = createdToken.ValidTo; // Or descriptor.Expires if you prefer
            dto.Token = token;

            return dto;
        }
    }
}