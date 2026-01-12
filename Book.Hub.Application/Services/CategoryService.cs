using Books.Hub.Application.Common.Exceptions;
using Books.Hub.Application.DTOs.Categories;
using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<CategoryDTO> GetByIdAsync(int Id , CancellationToken token)
        {
            var category = await _unitOfWork.Categories.GetById(Id , token);
            return category is null ?
                throw new NotFoundException($"Category with ID {Id} was not found.") :
                new CategoryDTO 
                {
                    Id = Id,
                    Name = category.Name,
                };
        }



        public async Task<IEnumerable<CategoryDTO>> GetAllAsync(CancellationToken token)
        {
            var spec = new QuerySpecification<Category>();
            var categories = await _unitOfWork.Categories.GetAll(spec, token);
            return categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name
            });
        }

        

        public async Task<CategoryDTO> CreateAsync(CreateCategoryDTO dto, CancellationToken token)
        {
            var category = await _unitOfWork.Categories.AddAsync(new Category { Name = dto.Name } , token);
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<CategoryDTO> EditAsync(CategoryDTO dto ,CancellationToken token)
        {
            var category = await _unitOfWork.Categories.GetById(dto.Id , token);

            if (category is null)
                throw new NotFoundException($"Category with ID {dto.Id} was not found.");

            GenericMapDtoToEntity(dto, category);

            await _unitOfWork.Categories.EditAsync(category ,token);
            return dto;
        }



        public async Task<bool> DeleteAsync(int Id, CancellationToken token)
        {
            var category = await _unitOfWork.Categories.GetById(Id, token);

            if (category is null)
                throw new NotFoundException($"Category with ID {Id} was not found.");
            
            return await _unitOfWork.Categories.DeleteAsync(category ,token);
        }

        public Task<IEnumerable<CategoryDTO>> GetAllAsync(QuerySpecification<Category> query, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
