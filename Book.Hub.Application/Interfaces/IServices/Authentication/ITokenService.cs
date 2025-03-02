using Books.Hub.Application.Comman;
using Books.Hub.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Interfaces.IServices.Authentication
{
    public interface ITokenService
    {
        Task<RegisterResultDTO> CreateTokenAsync(ApplicationUser user, RegisterResultDTO dto);

    }
}
