using Books.Hub.Application.Common.Exceptions;
using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Application.Identity;
using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Application.Mappers;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Books.Hub.Application.Services
{
    public class AuthorService : BaseService , IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthorService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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

            if(search.searchText is not null)
                spec.AddCriteria(a => a.Name.Contains(search.searchText));

            var authors = await _unitOfWork.Authors.GetAll(spec, token);
            return authors.Select(a => a.ToAuthorDTO()).ToList();
        }


        // get author by id
        public async Task<AuthorDTO> GetByIdAsync(int Id,CancellationToken token)
        {
            var spec = new QuerySpecification<Author>();
            spec.AddInclude(q => q.Include(a => a.Books));
            spec.AddInclude(q => q.Include(a => a.AuthorSubscribers));

            var author = await _unitOfWork.Authors.GetById(Id,spec, token)
                ?? throw new NotFoundException($"Author with ID {Id} was not found.");

            return author.ToAuthorDTO();
        }


        // get author by id without Includes
        public async Task<AuthorDTO> SimpleGetByIdAsync(int Id, CancellationToken token)
        {
            var author = await _unitOfWork.Authors.GetById(Id, token)
                ?? throw new NotFoundException($"Author with ID {Id} was not found.");

            return author.ToAuthorDTO();
        }


        // get author by id optimized
        public async Task<AuthorDTO> GetByIdAsync1(int Id, CancellationToken token)
        {
            return await _unitOfWork.Authors.GetByIdAsyncOptimized(Id, token)
                ?? throw new NotFoundException($"Author with ID {Id} was not found.");
        }


        // create author profile by Admin
        public async Task<AuthorDTO> CreateAuthorProfile(CreateAuthorDTO dto, CancellationToken token)
        {
            // handle Author profile cover
            if (dto.AuthorImageFile is not null)
                dto.AuthorImage = await HandleImageFiles(dto.AuthorImageFile);

            var newProfile = new Author()
            {
                Name = dto.Name,
                AuthorImage = dto.AuthorImage,
                Bio = dto.Bio,
                Nationality = dto.Nationality,
                DateOfBrith = dto.DateOfBrith,
                HaveAccount = false,
                IsActive = false,
                ApplicationAuthorId = null             
            };

            var author = await _unitOfWork.Authors.AddAsync(newProfile, token);
            return author.ToAuthorDTO();
        }


        // create author profile by Author User
        public async Task<AuthorDTO> CreateAuthorProfile(string id ,CreateAuthorDTO dto, CancellationToken token)
        {
            var authorUser = await _userManager.FindByIdAsync(id);

            if (authorUser is null)
                throw new NotFoundException($"No Author User exist with id: {id}");

            // handle Author profile cover
            if (dto.AuthorImageFile is not null)
                dto.AuthorImage = await HandleImageFiles(dto.AuthorImageFile);

            var newProfile = new Author()
            {
                Name = dto.Name,
                AuthorImage = dto.AuthorImage,
                Bio = dto.Bio,
                Nationality = dto.Nationality,
                DateOfBrith = dto.DateOfBrith,
                HaveAccount = true,
                IsActive = true,
                ApplicationAuthorId = id
            };

            var author = await _unitOfWork.Authors.AddAsync(newProfile, token);
            return author.ToAuthorDTO();
        }



        public async Task<AuthorDTO> EditAsync(EditAuthorDTO dto , CancellationToken token)
        {
            var author = await _unitOfWork.Authors.GetById(dto.Id , token);
            if (author is null)
                throw new NotFoundException($"No Author Profile exist with id: {dto.Id}");

            if (dto.AuthorImageFile is not null)
                dto.AuthorImage = await HandleImageFiles(dto.AuthorImageFile);

            GenericMapDtoToEntity(dto, author);

            await _unitOfWork.Authors.EditAsync(author , token);
            return author.ToAuthorDTO();
        }



        public async Task<bool> DeleteAsync(int Id , CancellationToken token)
        {
            var author = await _unitOfWork.Authors.GetById(Id , token);
            if (author is null)
                throw new NotFoundException($"No Author Profile exist with id: {Id}");

            return await _unitOfWork.Authors.DeleteAsync(author , token);
        }
    }
}
