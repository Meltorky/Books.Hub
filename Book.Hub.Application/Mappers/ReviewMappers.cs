using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Hub.Application.DTOs.BookReviews;
using Books.Hub.Domain.Entities;

namespace Books.Hub.Application.Mappers
{
    public static class ReviewMappers
    {
        public static BookReview ToBookReview(this AddReviewDTO dto) 
        {
            return new BookReview 
            {
                BookId = dto.BookId,
                Rating = dto.Rating,
                UserId = dto.UserId,
                Comment = dto.Comment,
                CreatedAt = DateTime.Now,
            };
        }


        public static ReviewDTO ToReviewDTO(this BookReview dto)
        {
            return new ReviewDTO 
            {
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = dto.CreatedAt  
            };
        }
    }
}
