using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Domain.Entities;
using Books.Hub.Infrastructure.Data;

namespace Books.Hub.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IAuthorRepository Authors { get; private set; }
        public IBookRepository Books { get; private set; }
        public ICategoryRepository Categories { get; private set; }

        public UnitOfWork(
            AppDbContext context,
            IBookRepository books,
            IAuthorRepository authors,
            ICategoryRepository categories)
        {
            _context = context;
            Authors = authors;
            Books = books;
            Categories = categories;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }


        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
