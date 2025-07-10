using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.DTOs.Auth
{
    public class RevokeTokenDTO
    {
        public string Token { get; set; } = string.Empty;
    }
}
