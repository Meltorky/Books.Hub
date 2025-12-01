using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Interfaces.IServices
{
    public interface IAuthorService
    {
        Task<AuthorDTO> SimpleGetByIdAsync(int Id, CancellationToken token);
        Task<AuthorDTO> GetByIdAsync(int Id, CancellationToken token);
        Task<List<AuthorDTO>> GetAllAsync(AdvancedSearch search, CancellationToken token);
        Task<AuthorDTO> CreateAuthorProfile(string? id , CreateAuthorDTO dto , CancellationToken token);
        Task<AuthorDTO> EditAsync(EditAuthorDTO dto ,CancellationToken token);
        Task<bool> DeleteAsync(int Id, CancellationToken token);
    }
}
