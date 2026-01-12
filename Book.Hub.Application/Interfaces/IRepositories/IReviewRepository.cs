using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Hub.Domain.Entities;

namespace Books.Hub.Application.Interfaces.IRepositories
{
    public interface IReviewRepository : IBaseRepository<BookReview>
    {
        Task<BookReview?> FindBookReviewAsync(string userId, int bookId, CancellationToken token);
    }
}
