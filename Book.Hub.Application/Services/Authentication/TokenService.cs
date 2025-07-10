using Books.Hub.Application.DTOs.Auth;
using Books.Hub.Application.Identity;
using Books.Hub.Application.Interfaces.IServices.Authentication;
using Books.Hub.Appliction.Options;
using Books.Hub.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
            //dto.Message = "Token created successfully !!";
            //dto.IsAuthenticated = true;
            //dto.ExpiresOn = createdToken.ValidTo; // Or descriptor.Expires
            //dto.Token = token;

            // Map to RegisterResultDTO

            dto.Message = "Token created successfully";
            dto.IsAuthenticated = true;
            dto.Token = token;
            dto.ExpiresOn = createdToken.ValidTo; 

            // Return RefreshToken Date in api, and Refresh Token in Cookie [in memory => JSONIgnore]

            if (user.RefreshTokens is not null && user.RefreshTokens.Any(r => r.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(r => r.IsActive);
                dto.RefreshToken = activeRefreshToken!.Token;
                dto.RefreshTokenExpiresOn = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                dto.RefreshToken = refreshToken!.Token;
                dto.RefreshTokenExpiresOn = refreshToken.ExpiresOn;
                user.RefreshTokens!.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return dto;
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