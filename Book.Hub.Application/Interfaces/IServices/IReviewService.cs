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
        
        Task<ReviewDTO> GetById(int Id,CancellationToken token);

        Task<List<ReviewDTO>> GetAllForBook(int BookId,CancellationToken token);
        Task<List<ReviewDTO>> GetAllForUser(string UserId,CancellationToken token);

        Task<ReviewDTO> AddReview(AddReviewDTO dto,CancellationToken token);
        Task<ReviewDTO> EditReview( int Id, AddReviewDTO dto,CancellationToken token);
        Task<bool> Delete(int Id,CancellationToken token);
    }
}
