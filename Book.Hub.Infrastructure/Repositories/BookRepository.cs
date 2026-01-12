using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Domain.Entities;
using Books.Hub.Infrastructure.Data;

namespace Books.Hub.Infrastructure.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly AppDbContext _context;
        public BookRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
