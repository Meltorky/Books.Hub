using Books.Hub.Domain.Common;
using System.Security.Cryptography;

namespace Books.Hub.Application.Helpers
{
    public static class RefreshTokensHelper
    {
        public static RefreshToken GenerateRefreshToken()
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
