using Books.Hub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Interfaces.IRepositories
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<Book?> GetBookByIdAsync(int Id);
        Task RemoveBookCategories(ICollection<BookCategory> bookCategories);
        Task RemoveBookCategories(Book book);
    }
}
