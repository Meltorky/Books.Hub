﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.DTOs.Categories
{
    public class CreateCategoryDTO
    {
        [Required]
        [MaxLength(80)]
        public string Name { get; set; } = string.Empty;
    }
}
