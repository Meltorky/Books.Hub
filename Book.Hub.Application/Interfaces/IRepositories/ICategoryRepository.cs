using Books.Hub.Application.DTOs.Categories;
using Books.Hub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Interfaces.IRepositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<List<CategoryDTO>> GetAllAsync(CancellationToken token);
    }
}
