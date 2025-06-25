using Books.Hub.Application.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Interfaces.IServices.Admin
{
    public interface ICategoryService
    {
        Task<CategoryDTO?> GetByIdAsync(int Id);
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO> CreateAsync(CreateCategoryDTO dto);
        Task<CategoryDTO?> EditAsync(CategoryDTO dto);
        Task<bool> DeleteAsync(int Id);
    }
}
