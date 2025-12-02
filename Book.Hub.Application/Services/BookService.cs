using Books.Hub.Application.Common.Exceptions;
using Books.Hub.Application.DTOs.Books;
using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Application.Mappers;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.Hub.Application.Services
{
    public class BookService : BaseService, IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // get all books
        public async Task<List<BookDTO>> GetAllAsync
            (AdvancedSearch adv,
            int? categoryId,
            double? minPrice,
            double? maxPrice,
            CancellationToken token)
        {
            var spec = new QuerySpecification<Book>();

            spec.Skip = adv.Skip;
            spec.Take = adv.Take;
            spec.OrderByDescending = adv.IsDesc;
            spec.OrderBy = adv.SortedBy switch
            {
                "best-seller" => b => b.TotalCopiesSold,
                "rating" => b => b.Rating,
                "name" => b => b.Name,
                "recentley-added" => b => b.PublishedDate,
                "price" => b => b.Price,
                _ => b => b.TotalCopiesSold
            };

            if (adv.searchText is not null)
                spec.AddCriteria(b => b.Name.Contains(adv.searchText.ToLowerInvariant()));

            if (minPrice.HasValue)
                spec.AddCriteria(b => b.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                spec.AddCriteria(b => b.Price <= maxPrice.Value);

            if (categoryId.HasValue)
                spec.AddCriteria(b => b.BookCategories.Any(bc => bc.CategoryId == categoryId));

            spec.AddInclude(q => q
                .Include(b => b.Author));

            spec.AddInclude(q => q
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category));

            var books = await _unitOfWork.Books.GetAll(spec, token);
            return books.Select(book => book.ToBookDTO()).ToList();
        }


        // get book by Id with details
        public async Task<BookDTO> GetByIdAsync(int id, CancellationToken token)
        {
            var spec = new QuerySpecification<Book>();
   
            spec.AddInclude(q => q.Include(b => b.Author));
            spec.AddInclude(q => q
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            );

            var book = await _unitOfWork.Books.GetById(
                id, spec, token)
                ?? throw new NotFoundException($"Book with ID {id} was not found.");

            return book.ToBookDTO();
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
            var book = await _unitOfWork.Books.GetById(dto.Id, token);

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
