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
    public class ReviewRepository : BaseRepository<BookReview> , IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
