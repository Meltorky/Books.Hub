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
        Task<AuthorDTO> GetByIdAsync(int Id, QuerySpecification<Author>? spec, CancellationToken token);
        Task<IEnumerable<AuthorDTO>> GetAllAsync(QuerySpecification<Author> spec, CancellationToken token);
        Task<AuthorDTO> CreateAuthorProfile(CreateAuthorDTO dto , CancellationToken token);
        Task<AuthorDTO> CreateAuthorProfile(string id , CreateAuthorDTO dto , CancellationToken token);
        Task<AuthorDTO> EditAsync(EditAuthorDTO dto ,CancellationToken token);
        Task<bool> DeleteAsync(int Id, CancellationToken token);
    }
}
