using Books.Hub.Application.Common.Exceptions;
using Books.Hub.Application.DTOs.Books;
using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Application.Interfaces.IServices.Comman;
using Books.Hub.Application.Mappers;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.Hub.Application.Services
{
    public class BookService : BaseService, IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageUploadService _image;
        public BookService(IUnitOfWork unitOfWork, IImageUploadService image)
        {
            _unitOfWork = unitOfWork;
            _image = image;
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
                "top-rated" => b => b.Rating,
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

       
        public async Task<BookDTO> CreateBookAsync(FormBookDTO dto, CancellationToken token)
        {
            // 1. Validate Author
            var author = await _unitOfWork.Authors.GetByIdFast(dto.AuthorId, token)
                ?? throw new ArgumentException($"Author with ID {dto.AuthorId} does not exist.");

            // 2. Validate Categories
            var distinctCategoryIds = dto.BookCategoryIDs.Distinct().ToList();
            var existingCategory = await _unitOfWork.Categories.GetRange(distinctCategoryIds, token);
            var existingCategoryIds = existingCategory.Select(c => c.Id).ToList();

            var invalidCategoryIds = distinctCategoryIds.Except(existingCategoryIds).ToList();

            if (invalidCategoryIds.Any())
                throw new OperationFailedException($"Invalid Category IDs: {string.Join('|', invalidCategoryIds)}");

            // 3. Validate Book Title should not be duplicate in DB
            if (await _unitOfWork.Books.IsExitAsync(a => a.Name == dto.Name, token))
                throw new OperationCanceledException($"{dto.Name} Book Name Is Already Exist.");

            // 4. save book cover in ImageKit
            if (dto.BookCover is not null)
            {
                var uploadedCover = await _image.UploadAsync(dto.BookCover, $"{dto.Name}Cover", true);
                dto.BookCoverID = uploadedCover.uploadedFileId;
                dto.BookCoverURL = uploadedCover.uploadedFileURL;
            }

            // 5. save book file in ImageKit
            if (dto.BookFile is not null)
            {
                var uploadedFile = await _image.UploadAsync(dto.BookFile, $"{dto.Name}file", false);
                dto.BookFileID = uploadedFile.uploadedFileId;
                dto.BookFileURL = uploadedFile.uploadedFileURL;
            }

            // 6. create new Book
            var result = await _unitOfWork.Books.AddAsync(dto.ToBook(), token);

            // 7. Put the new book in the Redis Cache

            return result.ToBookDTO(author.Name, existingCategory);
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


        public async Task<BookDTO> EditAsync(int Id, FormBookDTO dto, CancellationToken token)
        {
            var book = await _unitOfWork.Books.GetByIdFast(Id, token)
                ?? throw new NotFoundException($"Book with ID {Id} was not found.");

            // Validate Author
            var author = await _unitOfWork.Authors.GetByIdFast(dto.AuthorId, token)
                ?? throw new ArgumentException($"Author with ID {dto.AuthorId} does not exist.");

            // Validate Categories
            var distinctCategoryIds = dto.BookCategoryIDs.Distinct().ToList();
            var existingCategoryIds = await _unitOfWork.Categories
                .GetExistingIdsRange(distinctCategoryIds, token);

            var invalidCategoryIds = distinctCategoryIds.Except(existingCategoryIds).ToList();

            if (invalidCategoryIds.Any())
                throw new ArgumentException($"Invalid Category IDs: {string.Join('|', invalidCategoryIds)}");

            // Validate Book Title should not be duplicate in DB
            if (book.Name != dto.Name && await _unitOfWork.Books.IsExitAsync(a => a.Name == dto.Name, token))
                throw new ArgumentException($"{dto.Name} Book Name Is Already Exist.");

            // save book cover in ImageKit
            if (dto.BookCover is not null)
            {
                var uploadedCover = await _image.UploadAsync(dto.BookCover, $"{dto.Name}Cover", true);
                dto.BookCoverID = uploadedCover.uploadedFileId;
                dto.BookCoverURL = uploadedCover.uploadedFileURL;
            }
            else
            {
                dto.BookCoverURL = book.BookCoverURL;
                dto.BookCoverID = book.BookCoverId;
            }

            // save book file in ImageKit
            if (dto.BookFile is not null)
            {
                var uploadedFile = await _image.UploadAsync(dto.BookFile, $"{dto.Name}file", false);
                dto.BookFileID = uploadedFile.uploadedFileId;
                dto.BookFileURL = uploadedFile.uploadedFileURL;
            }
            else
            {
                dto.BookFileID = book.BookFileId;
                dto.BookFileURL = book.BookFileURL;
            }

            // map the dto values to the orginal book
            book.MapNewValues(dto);

            // create new Book
            var result = await _unitOfWork.Books.EditAsync(book, token);

            return result.ToBookDTO(author.Name);
        }


        public async Task<bool> DeleteAsync(int Id, CancellationToken token)
        {
            var book = await _unitOfWork.Books.GetById(Id, token)
                ?? throw new NotFoundException($"Book with ID {Id} was not found.");

            if(book.BookFileId is not null) await DeleteImageKitFiles(book.BookFileId);
            if(book.BookCoverId is not null) await DeleteImageKitFiles(book.BookCoverId);

            return await _unitOfWork.Books.DeleteAsync(book, token);
        }


        private async Task<List<string>> DeleteImageKitFiles(params string[] fileIds)
        {
            List<string> ids = new List<string>();
            foreach (var fileId in fileIds)
                ids.Add(await _image.DeleteAsync(fileId));

            return ids;
        }
    }
}