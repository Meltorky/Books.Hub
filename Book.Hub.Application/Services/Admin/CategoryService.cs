﻿using Books.Hub.Application.DTOs.Admin;
using Books.Hub.Application.Interfaces.IRepositories.Admin;
using Books.Hub.Application.Interfaces.IServices.Admin;
using Books.Hub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Services.Admin
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDTO?> GetByIdAsync(int Id)
        {
            var category = await _categoryRepository.GetByIdAsync(Id);
            return category is null ? null : new CategoryDTO
            {
                Id = Id,
                Name = category.Name
            };
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync() 
        {
            var result = await _categoryRepository.GetAllAsync();
            return result.Select(c => new CategoryDTO 
            {
                Id= c.Id,
                Name = c.Name
            });
        }

        public async Task<CategoryDTO> CreateAsync(CreateCategoryDTO dto)
        {
            var category = await _categoryRepository.AddAsync(new Category { Name = dto.Name });
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<CategoryDTO?> EditAsync(CategoryDTO dto)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.Id);
            
            if (category is null)
                return null;

            GenericEditMethod(dto,category);

            await _categoryRepository.EditAsync(category);
            return new CategoryDTO 
            {
                Id = category.Id,
                Name = category.Name
            };
        }


        public async Task<bool> DeleteAsync(int Id) 
        {
            var category = await _categoryRepository.GetByIdAsync(Id);
            return category is null? 
                false : 
                await _categoryRepository.DeleteAsync(category);
        }

    }
}
