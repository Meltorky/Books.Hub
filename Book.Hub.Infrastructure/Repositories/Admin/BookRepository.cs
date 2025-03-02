using Books.Hub.Application.Interfaces.IRepositories.Admin;
using Books.Hub.Domain.Entities;
using Books.Hub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Infrastructure.Repositories.Admin
{
    public class BookRepository : BaseRepository<Book> , IBookRepository
    {
        private readonly AppDbContext _context;
        public BookRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<Book>> GetAllAsync()
        //{
        //    return await _context.Books
        //        .AsNoTracking()
        //        .OrderBy(x => x.Name)
        //        .ToListAsync();
        //}
    }
}
