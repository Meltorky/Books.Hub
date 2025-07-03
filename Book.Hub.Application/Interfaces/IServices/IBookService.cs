using Books.Hub.Application.DTOs.Books;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Interfaces.IServices
{
    public interface IBookService
    {
        Task<BookDTO> CreateBookAsync(CreateBookDTO dto, CancellationToken cancellationToken);
        Task<IEnumerable<BookDTO>> GetAllAsync(QuerySpecification<Book> spec, CancellationToken cancellationToken);
        Task<BookDTO> GetByIdAsync(int id, QuerySpecification<Book>? spec, CancellationToken cancellationToken);
        Task<BookDTO> EditAsync(EditBookDTO dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int Id, CancellationToken cancellationToken);
    }
}
