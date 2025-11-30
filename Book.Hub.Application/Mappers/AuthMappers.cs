using Books.Hub.Application.DTOs.Auth;
using Books.Hub.Application.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Mappers
{
    public static class AuthMappers
    {
        public static ApplicationUser ToAppUser(this RegisterDTO dto)
        {
            return new ApplicationUser
            {
                Email = dto.Email,
                UserName = $"@{new MailAddress(dto.Email).User.ToLowerInvariant()}",
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailConfirmed = true
            };
        }

        public static void bindAppUser(this RegisterResultDTO dto, ApplicationUser user)
        {
            dto.Email = user.Email!;
            dto.UserName = user.UserName!;
            dto.FirstName = user.FirstName;
            dto.LastName = user.LastName;
        }
    }
}
