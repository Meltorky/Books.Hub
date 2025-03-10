﻿using Books.Hub.Application.DTOs.Admin;
using Books.Hub.Application.Interfaces.IRepositories.Admin;
using Books.Hub.Application.Interfaces.IService.Admin;
using Books.Hub.Application.Interfaces.IServices.Admin;
using Books.Hub.Application.Mappers.Admin;
using Books.Hub.Application.Options;
using Books.Hub.Application.Services.Admin;
using Books.Hub.Domain.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Service.Admin
{
    public class AuthorService : BaseService , IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }


        public async Task<IEnumerable<AuthorDTO>> GetAllAsync()
        {
            var authors = await _authorRepository.GetAllAsync();
            return authors.Select(a => a.ToAuthorDTO());
        }

        public async Task<AuthorDTO?> GetByIdAsync(int Id)
        {
            var author = await _authorRepository.GetByIdAsync(Id);

            return author is null ? null : author.ToAuthorDTO();
        }

        public async Task<AuthorDTO?> AddAsync(CreateAuthorDTO dto)
        {
            byte[]? authorImage = null; 

            if (dto.AuthorImageFile is not null)
                dto.AuthorImage = await HandleImageFiles(dto.AuthorImageFile);

            var author = await _authorRepository.AddAsync(dto.CreateAuthorDTOToAuthor(authorImage));
            return author is null ? null : author.ToAuthorDTO();
        }

        public async Task<AuthorDTO?> EditAsync(EditAuthorDTO dto)
        {
            var author = await _authorRepository.GetByIdAsync(dto.Id);
            if (author == null)
                return null; // Author not found, return null

            if (dto.AuthorImageFile is not null)
                dto.AuthorImage = await HandleImageFiles(dto.AuthorImageFile);

            GenericEditMethod(dto, author, nameof(dto.DateOfBrith));

            await _authorRepository.EditAsync(author);
            return author.ToAuthorDTO();
        }


        public async Task<bool> DeleteAsync(int Id) 
        {
            var author = await _authorRepository.GetByIdAsync(Id);
            if(author is not null && await _authorRepository.DeleteAsync(author))
                return true;

            return false;
        }

    }
}
