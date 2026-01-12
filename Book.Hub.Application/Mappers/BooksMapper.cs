using Books.Hub.Application.DTOs.Books;
using Books.Hub.Application.DTOs.Categories;
using Books.Hub.Domain.Entities;

namespace Books.Hub.Application.Mappers
{
    public static class BooksMapper
    {
        public static Book ToBook(this FormBookDTO dto) 
        {
            return new Book()
            {
                Name = dto.Name,
                Description = dto.Description,
                Language = dto.Language,
                PageCount = dto.PageCount,    
                Price = dto.Price,
                Rating = dto.Rating,
                TotalCopiesSold = dto.TotalCopiesSold,
                AuthorId = dto.AuthorId,
                PublishedDate = dto.PublishedDate,
                BookCoverURL = dto.BookCoverURL,
                BookCoverId = dto.BookCoverID,
                BookFileURL = dto.BookFileURL,
                BookFileId = dto.BookFileID,
                BookCategories = dto.BookCategoryIDs.Select(i => new BookCategory { CategoryId = i}).ToList()
            };
        }


        public static void MapNewValues(this Book book, FormBookDTO dto)
        {
            book.Name = dto.Name;
            book.Description = dto.Description;
            book.Language = dto.Language;
            book.PageCount = dto.PageCount;
            book.Price = dto.Price;
            book.Rating = dto.Rating;
            book.TotalCopiesSold = dto.TotalCopiesSold;
            book.AuthorId = dto.AuthorId;
            book.PublishedDate = dto.PublishedDate;
            book.BookCoverURL = dto.BookCoverURL;
            book.BookCoverId = dto.BookCoverID;
            book.BookFileURL = dto.BookFileURL;
            book.BookFileId = dto.BookFileID;
            book.BookCategories = dto.BookCategoryIDs.Select(i => new BookCategory { CategoryId = i }).ToList();
        }



        public static BookDTO ToBookDTO(this Book model)
        {
            return new BookDTO()
            {
                Id = model.Id,
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
                BookCoverURL = model.BookCoverURL,
                BookFileURL = model.BookFileURL,
                BookCategories = model.BookCategories.Select(i => new CategoryDTO
                { 
                    Id = i.Category.Id,
                    Name = i.Category.Name
                }).ToList()
            };
        }

        public static BookDTO ToBookDTO(this Book model, string authorName)
        {
            return new BookDTO()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IsAvailable = model.IsAvailable,
                Language = model.Language,
                PageCount = model.PageCount,
                Price = model.Price,
                Rating = model.Rating,
                TotalCopiesSold = model.TotalCopiesSold,
                AuthorId = model.AuthorId,
                AuthorName = authorName,
                BookCoverURL = model.BookCoverURL,
                BookFileURL = model.BookFileURL,
                BookCategories = new List<CategoryDTO>() 
            };
        }

        public static BookDTO ToBookDTO(this Book model, string authorName, List<Category> categories)
        {
            return new BookDTO()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IsAvailable = model.IsAvailable,
                Language = model.Language,
                PageCount = model.PageCount,
                Price = model.Price,
                Rating = model.Rating,
                TotalCopiesSold = model.TotalCopiesSold,
                AuthorId = model.AuthorId,
                AuthorName = authorName,
                BookCoverURL = model.BookCoverURL,
                BookFileURL = model.BookFileURL,
                BookCategories = categories.Select(x => new CategoryDTO { Id = x.Id, Name = x.Name}).ToList()
            };
        }
    }
}
