using Books.Hub.Application.DTOs.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Interfaces.IService.Admin
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDTO>> GetAllAsync();
        Task<AuthorDTO?> GetByIdAsync(int Id);
        Task<AuthorDTO?> AddAsync(CreateAuthorDTO dto);
        Task<AuthorDTO?> EditAsync(EditAuthorDTO dto);
        Task<bool> DeleteAsync(int Id);
    }
}
