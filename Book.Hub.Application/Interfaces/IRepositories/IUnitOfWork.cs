using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Hub.Domain.Entities;

namespace Books.Hub.Application.Interfaces.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthorRepository Authors { get; }
        IBookRepository Books { get; }
        ICategoryRepository Categories { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
