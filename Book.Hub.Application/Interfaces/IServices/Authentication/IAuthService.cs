using Books.Hub.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Interfaces.IServices.Authentication
{
    public interface IAuthService
    {
        Task<RegisterResultDTO> RegisterAsync(RegisterDTO dto);
        Task<RegisterResultDTO> LoginAsync(LoginDTO dto);
        Task<RegisterResultDTO> RefreshTokenAsyncHandler(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}
