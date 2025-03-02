﻿using Books.Hub.Application.DTOs.Admin;
using Books.Hub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Mappers.Admin
{
    public static class AuthorMappers
    {
        public static AuthorDTO ToAuthorDTO(this Author model) 
        {
            return new AuthorDTO
            {
                Id = model.Id,
                Name = model.Name,
                Bio = model.Bio,
                DateOfBrith = model.DateOfBrith.ToString("yyyy-MM-dd"),
                IsActive = model.IsActive,
                Nationality = model.Nationality,
                AuthorImage = model.AuthorImage?.Take(50).ToArray(),  // .Take(50) => this is just for simplification while production, Not used in the real cases
            };
        }

        public static Author ToAuthor(this AuthorDTO dto)
        {
            return new Author
            {
                Id = dto.Id,
                Name = dto.Name,
                Bio = dto.Bio,
                DateOfBrith = DateOnly.Parse(dto.DateOfBrith),
                IsActive = dto.IsActive,
                Nationality = dto.Nationality,
            };
        }

        public static Author CreateAuthorDTOToAuthor(this CreateAuthorDTO dto , byte[]? authorImage = null)
        {
            return new Author
            {
                Name = dto.Name,
                Bio = dto.Bio,
                DateOfBrith = DateOnly.Parse(dto.DateOfBrith),
                IsActive = dto.IsActive,
                Nationality = dto.Nationality,
                AuthorImage = authorImage ?? null
            };
        }

       
    }
}
