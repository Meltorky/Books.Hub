using Books.Hub.Application.Common.Exceptions;
using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Application.Identity;
using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Application.Interfaces.IServices.Comman;
using Books.Hub.Application.Mappers;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Books.Hub.Application.Services
{
    public class AuthorService : BaseService, IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageUploadService _image;
        public AuthorService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IImageUploadService image)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _image = image;
        }



        // get all authors
        public async Task<List<AuthorDTO>> GetAllAsync(AdvancedSearch search, CancellationToken token)
        {
            var spec = new QuerySpecification<Author>();
            spec.AddInclude(q => q.Include(a => a.Books));
            spec.AddInclude(q => q.Include(a => a.AuthorSubscribers));
            spec.Skip = search.Skip;
            spec.Take = search.Take;
            spec.OrderByDescending = search.IsDesc;
            spec.OrderBy = search.SortedBy switch
            {
                "trending" => a => a.Books.Sum(i => i.TotalCopiesSold),
                "popular" => a => a.AuthorSubscribers.Count(),
                _ => a => a.Name,
            };

            if (search.searchText is not null)
                spec.AddCriteria(a => a.Name.Contains(search.searchText));

            var authors = await _unitOfWork.Authors.GetAll(spec, token);
            return authors.Select(a => a.ToAuthorDTO()).ToList();
        }



        // get author by id without Includes
        public async Task<AuthorDTO> SimpleGetByIdAsync(int Id, CancellationToken token)
        {
            var author = await _unitOfWork.Authors.GetById(Id, token)
                ?? throw new NotFoundException($"Author with ID {Id} was not found.");

            return author.ToAuthorDTO();
        }



        // get author by id
        public async Task<AuthorDTO> GetByIdAsync(int Id, CancellationToken token)
        {
            var spec = new QuerySpecification<Author>();
            spec.AddInclude(q => q.Include(a => a.Books));
            spec.AddInclude(q => q.Include(a => a.AuthorSubscribers));

            var author = await _unitOfWork.Authors.GetById(Id, spec, token)
                ?? throw new NotFoundException($"Author with ID {Id} was not found.");

            return author.ToAuthorDTO();
        }



        // get author by id optimized
        public async Task<AuthorDTO> GetByIdAsync1(int Id, CancellationToken token)
        {
            return await _unitOfWork.Authors.GetByIdAsyncOptimized(Id, token)
                ?? throw new NotFoundException($"Author with ID {Id} was not found.");
        }



        // create author profile by Author User / Admin
        public async Task<AuthorDTO> CreateAuthorProfile(string? userId, CreateAuthorDTO dto, CancellationToken token)
        {
            if (userId is not null)
                if (await _userManager.FindByIdAsync(userId) is null)
                    throw new NotFoundException($"No User exist with id: {userId}");

            // handle Author profile cover
            if (dto.AuthorImageFile is not null)
                dto.AuthorImageURL = await _image
                    .UploadAsync(dto.AuthorImageFile, dto.Name.Trim().ToLowerInvariant(), true);

            var newProfile = new Author()
            {
                Name = dto.Name,
                AuthorImageURL = dto.AuthorImageURL,
                Bio = dto.Bio,
                Nationality = dto.Nationality,
                DateOfBrith = dto.DateOfBrith,
                HaveAccount = userId is null ? false : true,
                IsActive = userId is null ? false : true,
                ApplicationAuthorId = userId
            };

            var author = await _unitOfWork.Authors.AddAsync(newProfile, token);
            return author.ToAuthorDTO();
        }



        // Edit author
        public async Task<AuthorDTO> EditAsync(EditAuthorDTO dto, CancellationToken token)
        {
            var author = await _unitOfWork.Authors.GetByIdFast(dto.Id, token)
                ?? throw new NotFoundException($"No Author Profile exist with id: {dto.Id}");

            // handle Author profile cover
            if (dto.AuthorImageFile is not null)
                author.AuthorImageURL = await _image
                    .UploadAsync(dto.AuthorImageFile, dto.Name.Trim().ToLowerInvariant(), true);

            author.Name = dto.Name;
            author.Bio = dto.Bio;
            author.Nationality = dto.Nationality;
            author.IsActive = dto.IsActive;
            author.DateOfBrith = dto.DateOfBrith;

            return (await _unitOfWork.Authors.EditAsync(author, token)).ToAuthorDTO();
        }



        // delete author
        public async Task<bool> DeleteAsync(int Id, CancellationToken token)
        {
            var author = await _unitOfWork.Authors.GetById(Id, token, a => a.Include(a => a.Books))
                ?? throw new NotFoundException($"No Author Profile exist with id: {Id}");

            if (author.Books.Count() > 0)
                throw new OperationCanceledException($"You should remove all related books first !!");

            return await _unitOfWork.Authors.DeleteAsync(author, token);
        }
    }
}
