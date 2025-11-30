using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Interfaces.IRepositories
{
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        Task<AuthorDTO?> GetByIdAsyncOptimized(int id, CancellationToken token);
    }
}
