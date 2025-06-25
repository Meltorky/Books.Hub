using Books.Hub.Application.DTOs.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Interfaces.IServices.Admin
{
    public interface IBookService
    {
        Task<BookDTO?> CreateBookAsync(CreateBookDTO dto);
        Task<IEnumerable<BookDTO>> GetAllAsync();
        Task<BookDTO?> GetByIdAsync(int Id);
        Task<BookDTO?> EditAsync(EditBookDTO dto);
        Task<bool> DeleteAsync(int Id);
    }
}
