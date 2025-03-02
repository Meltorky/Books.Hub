using Books.Hub.Application.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Interfaces.IServices.Admin
{
    public interface IBookService
    {
        Task<BookDTO> CreateBookAsync(CreateBookDTO dto);
    }
}
