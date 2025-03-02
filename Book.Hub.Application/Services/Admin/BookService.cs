using Books.Hub.Application.DTOs.Admin;
using Books.Hub.Application.Interfaces.IRepositories.Admin;
using Books.Hub.Application.Interfaces.IServices.Admin;
using Books.Hub.Application.Mappers.Admin;
using Books.Hub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Services.Admin
{
    public class BookService : BaseService , IBookService
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

        public async Task<BookDTO> CreateBookAsync(CreateBookDTO dto) 
        {
            // 1. Validate Author
            if (!await _authorRepository.IsExitAsync(a => a.Id == dto.AuthorId))
            {
                throw new ArgumentException($"Author with ID {dto.AuthorId} does not exist.");
            }

            bool isAuthorExist = await _authorRepository.IsExitAsync(a => a.Id == dto.AuthorId);

            // 2. Validate Categories
            var categoryIds = (await _categoryRepository.GetAllAsync()).Select(c => c.Id).ToList();
            var invalidCategoryIds = dto.BookCategoryIDs.Except(categoryIds).ToList();

            if (invalidCategoryIds.Any())
            {
                throw new ArgumentException($"Invalid Category IDs: {string.Join(", ", invalidCategoryIds)}");
            }

            // 3. Validate Book Title should not be duplicate in DB
            if (await _bookRepository.IsExitAsync(a => a.Name == dto.Name))
            {
                throw new ArgumentException($"{dto.Name} Book Name Is Already Exist.");
            }

            // 4. Convert Book Cover FormFile To byte[]
            if(dto.BookCoverFile is not null)
                dto.BookCover =  await HandleImageFiles(dto.BookCoverFile);

            // 5. create new Book
            var result = await _bookRepository.AddAsync(dto.CreateBookDTOtoBook());
            //result = null;
            // map to BookDTO

            return result.ToBookDTO();
        }
    }
}
