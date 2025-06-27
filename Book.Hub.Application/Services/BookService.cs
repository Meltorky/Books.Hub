using Books.Hub.Application.Common.Exceptions;
using Books.Hub.Application.DTOs.Books;
using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Application.Mappers;
using Books.Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Services
{
    public class BookService : BaseService, IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<BookDTO>> GetAllAsync(CancellationToken cancellationToken)
        {
            var books = await _bookRepository.GetAllAsync(

                q => q.Include(e => e.Author).ThenInclude(a => a.Books),
                q => q.Include(e => e.BookCategories).ThenInclude(a => a.Category)

            );
            return books.Select(book => book.ToBookDTO());
        }


        public async Task<BookDTO> GetByIdAsync(int Id, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookByIdAsync(Id);
            return book is null ?
                throw new NotFoundException($"Book with ID {Id} was not found.") :
                book.ToBookDTO();
        }


        public async Task<BookDTO> CreateBookAsync(CreateBookDTO dto, CancellationToken cancellationToken)
        {
            // 1. Validate Author
            if (!await _authorRepository.IsExitAsync(a => a.Id == dto.AuthorId))
                throw new ArgumentException($"Author with ID {dto.AuthorId} does not exist.");

            // 2. Validate Categories
            var categoryIds = (await _categoryRepository.GetAllAsync()).Select(c => c.Id).ToList();

            var invalidCategoryIds = dto.BookCategoryIDs.Except(categoryIds).ToList();
            if (invalidCategoryIds.Any())
                throw new ArgumentException($"Invalid Category IDs: {string.Join(", ", invalidCategoryIds)}");

            // 3. Validate Book Title should not be duplicate in DB
            if (await _bookRepository.IsExitAsync(a => a.Name == dto.Name))
                throw new ArgumentException($"{dto.Name} Book Name Is Already Exist.");

            // 4. Convert Book Cover FormFile To byte[]
            if (dto.BookCoverFile is not null)
                dto.BookCover = await HandleImageFiles(dto.BookCoverFile);

            // 5. create new Book
            var result = await _bookRepository.AddAsync(dto.CreateBookDTOtoBook());
            return result.ToBookDTO();

        }


        public async Task<BookDTO> EditAsync(EditBookDTO dto, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookByIdAsync(dto.Id);

            if (book is null)
                throw new NotFoundException($"Book with ID {dto.Id} was not found.");

            if (dto.BookCoverFile is not null)
                dto.BookCover = await HandleImageFiles(dto.BookCoverFile);

            GenericMapDtoToEntity(dto, book); // map the book dto to the book entity

            if (dto.BookCategoryIDs is not null)
            {
                // Validate Categories
                var categoryIds = (await _categoryRepository.GetAllAsync()).Select(c => c.Id).ToList();

                var invalidCategoryIds = dto.BookCategoryIDs.Except(categoryIds).ToList();
                if (invalidCategoryIds.Any())
                    throw new ArgumentException($"Invalid Category IDs: {string.Join(", ", invalidCategoryIds)}");

                // remove all book.BookCategories, then asgin the new values
                await _bookRepository.RemoveBookCategories(book.BookCategories);
                book.BookCategories = dto.BookCategoryIDs.Select(i => new BookCategory { CategoryId = i }).ToList();
            }

            await _bookRepository.EditAsync(book);
            return book.ToBookDTO();
        }


        public async Task<bool> DeleteAsync(int Id, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(Id);

            if (book is null) throw new NotFoundException($"Book with ID {Id} was not found.");

            return await _bookRepository.DeleteAsync(book);
        }
    }
}
