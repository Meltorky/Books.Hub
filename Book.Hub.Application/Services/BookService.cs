using Books.Hub.Application.Common.Exceptions;
using Books.Hub.Application.DTOs.Books;
using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Application.Mappers;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;

namespace Books.Hub.Application.Services
{
    public class BookService : BaseService, IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<IEnumerable<BookDTO>> GetAllAsync(QuerySpecification<Book> spec, CancellationToken cancellationToken)
        {
            var books = await _unitOfWork.Books.GetAll(spec, cancellationToken);
            return books.Select(book => book.ToBookDTO());
        }



        public async Task<BookDTO> GetByIdAsync(int id, QuerySpecification<Book>? spec, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetById(id, spec, cancellationToken);
            return book is null ?
                throw new NotFoundException($"Book with ID {id} was not found.") :
                book.ToBookDTO();
        }



        public async Task<BookDTO> CreateBookAsync(CreateBookDTO dto, CancellationToken token)
        {
            // 1. Validate Author
            if (!await _unitOfWork.Authors.IsExitAsync(a => a.Id == dto.AuthorId, token))
                throw new ArgumentException($"Author with ID {dto.AuthorId} does not exist.");

            // 2. Validate Categories
            var distinctCategoryIds = dto.BookCategoryIDs.Distinct().ToList();
            var existingCategoryIds = await _unitOfWork.Categories
                .GetExistingIdsRange(distinctCategoryIds, token);

            var invalidCategoryIds = distinctCategoryIds.Except(existingCategoryIds).ToList();

            if (invalidCategoryIds.Any())
                throw new ArgumentException($"Invalid Category IDs: {string.Join(", ", invalidCategoryIds)}");

            // 3. Validate Book Title should not be duplicate in DB
            if (await _unitOfWork.Books.IsExitAsync(a => a.Name == dto.Name, token))
                throw new ArgumentException($"{dto.Name} Book Name Is Already Exist.");

            // 4. Convert Book Cover FormFile To byte[]
            if (dto.BookCoverFile is not null)
                dto.BookCover = await HandleImageFiles(dto.BookCoverFile);

            // 5. create new Book
            var result = await _unitOfWork.Books.AddAsync(dto.CreateBookDTOtoBook(), token);
            return result.ToBookDTO();

        }


        public async Task<BookDTO> EditAsync(EditBookDTO dto, CancellationToken token)
        {
            //var spec = new QuerySpecification<Book>();

            //spec.AddInclude(q => q
            //    .Include(b => b.BookCategories)
            //    .ThenInclude(bc => bc.Category)
            //);

            //spec.AddInclude(q => q
            //    .Include(b => b.Author)
            //);

            //var book = await _unitOfWork.Books.GetById(dto.Id,spec,token);
            var book = await _unitOfWork.Books.GetById(dto.Id,token);

            if (book is null)
                throw new NotFoundException($"Book with ID {dto.Id} was not found.");

            if (dto.BookCoverFile is not null)
                dto.BookCover = await HandleImageFiles(dto.BookCoverFile);

            GenericMapDtoToEntity(dto, book); // map the book dto to the book entity

            if (dto.BookCategoryIDs is not null)
            {
                // Validate Categories
                var distinctCategoryIds = dto.BookCategoryIDs.Distinct().ToList();
                var existingCategoryIds = await _unitOfWork.Categories
                    .GetExistingIdsRange(distinctCategoryIds, token);

                var invalidCategoryIds = distinctCategoryIds.Except(existingCategoryIds).ToList();

                if (invalidCategoryIds.Any())
                    throw new ArgumentException($"Invalid Category IDs: {string.Join(", ", invalidCategoryIds)}");

                // remove all book.BookCategories, then asgin the new values
                await _unitOfWork.Books.RemoveBookCategories(book.BookCategories);
                book.BookCategories = dto.BookCategoryIDs.Select(i => new BookCategory { CategoryId = i }).ToList();
            }

            await _unitOfWork.Books.EditAsync(book, token);
            return book.ToBookDTO();
        }


        public async Task<bool> DeleteAsync(int Id, CancellationToken token)
        {
            var book = await _unitOfWork.Books.GetById(Id, token);

            if (book is null) throw new NotFoundException($"Book with ID {Id} was not found.");

            return await _unitOfWork.Books.DeleteAsync(book, token);
        }
    }
}
