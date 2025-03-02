﻿using Books.Hub.Application.DTOs.Admin;
using Books.Hub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Mappers.Admin
{
    public static class BooksMapper
    {
        public static Book CreateBookDTOtoBook(this CreateBookDTO dto) 
        {
            return new Book()
            {
                Name = dto.Name,
                Description = dto.Description,
                IsAvailable = dto.IsAvailable,
                Language = dto.Language,
                PageCount = dto.PageCount,    
                Price = dto.Price,
                Rating = dto.Rating,
                TotalCopiesSold = dto.TotalCopiesSold,
                AuthorId = dto.AuthorId,
                PublishedDate = DateOnly.Parse(dto.PublishedDate),
                BookCover = dto.BookCover,
                BookCategories = dto.BookCategoryIDs.Select(i => new BookCategory { CategoryId = i}).ToList()
            };
        }


        public static BookDTO ToBookDTO(this Book model)
        {
            return new BookDTO()
            {
                Name = model.Name,
                Description = model.Description,
                IsAvailable = model.IsAvailable,
                Language = model.Language,
                PageCount = model.PageCount,
                Price = model.Price,
                Rating = model.Rating,
                TotalCopiesSold = model.TotalCopiesSold,
                AuthorId = model.AuthorId,
                AuthorName = model.Author.Name,
                PublishedDate = model.PublishedDate,
                BookCover = model.BookCover?.Take(50).ToArray(),
                BookCategories = model.BookCategories.Select(i => i.Category.Name).ToList()
            };
        }
    }
}
