using Books.Hub.Application.Common.Exceptions;
using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Application.Identity;
using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Application.Mappers;
using Books.Hub.Application.Options;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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



        public async Task<IEnumerable<AuthorDTO>> GetAllAsync(QuerySpecification<Author> spec, CancellationToken token)
        {
            var authors = await _unitOfWork.Authors.GetAll(spec,token);
            return authors.Select(a => a.ToAuthorDTO());
        }



        public async Task<AuthorDTO> GetByIdAsync(int Id, QuerySpecification<Author>? spec,CancellationToken token)
        {
            var author = await _unitOfWork.Authors.GetById(Id, spec,token);

            return author is null ?
                throw new NotFoundException($"Author with ID {Id} was not found.") : 
                author.ToAuthorDTO();
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
