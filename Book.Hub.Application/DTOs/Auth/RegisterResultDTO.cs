﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Books.Hub.Application.DTOs.Auth
{
    public class RegisterResultDTO
    {
        public string Message { get; set; } = string.Empty;
        public bool IsAsginedToRole { get; set; }
        public bool IsAuthenticated { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public string Token { get; set; } = string.Empty;

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresOn { get; set; }


    }
}
