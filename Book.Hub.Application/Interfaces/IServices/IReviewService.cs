using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Hub.Application.DTOs.BookReviews;

namespace Books.Hub.Application.Interfaces.IServices
{
    public interface IReviewService
    {

        Task<ReviewDTO> GetById(string userId, int bookId, CancellationToken token);

        Task<List<ReviewDTO>> GetAllForBook(int BookId,CancellationToken token);
        Task<List<ReviewDTO>> GetAllForUser(string UserId,CancellationToken token);

        Task<ReviewDTO> AddReview(AddReviewDTO dto,CancellationToken token);
        Task<ReviewDTO> EditReview(string userId, int bookId, AddReviewDTO dto, CancellationToken token);
        Task<bool> Delete(string userId, int bookId, CancellationToken token);
    }
}
