﻿using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Mappers
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
                DateOfBrith = model.DateOfBrith,
                IsActive = model.IsActive,
                Nationality = model.Nationality,
                AuthorImage = model.AuthorImage?.Take(50).ToArray(),  // .Take(50) => this is just for simplification while production, Not used in the real cases
                Books = model.Books.Select(x => x.ToBookDTO()).ToList()
            };
        }

        public static Author ToAuthor(this AuthorDTO dto)
        {
            return new Author
            {
                Id = dto.Id,
                Name = dto.Name,
                Bio = dto.Bio,
                DateOfBrith = dto.DateOfBrith,
                IsActive = dto.IsActive,
                Nationality = dto.Nationality,
            };
        }

       
    }
}
