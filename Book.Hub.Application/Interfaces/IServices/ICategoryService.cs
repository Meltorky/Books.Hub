using Books.Hub.Application.DTOs.Categories;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Interfaces.IServices
{
    public interface ICategoryService
    {
        Task<CategoryDTO> GetByIdAsync(int Id , CancellationToken token);
        Task<IEnumerable<CategoryDTO>> GetAllAsync(QuerySpecification<Category> query, CancellationToken token);
        Task<IEnumerable<CategoryDTO>> GetAllAsync(CancellationToken token);
        Task<CategoryDTO> CreateAsync(CreateCategoryDTO dto , CancellationToken token);
        Task<CategoryDTO> EditAsync(CategoryDTO dto , CancellationToken token);
        Task<bool> DeleteAsync(int Id , CancellationToken token);
    }
}
