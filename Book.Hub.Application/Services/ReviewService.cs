using Books.Hub.Application.Common.Exceptions;
using Books.Hub.Application.DTOs.BookReviews;
using Books.Hub.Application.Identity;
using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Application.Mappers;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Books.Hub.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public ReviewService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }



        public async Task<ReviewDTO> AddReview(AddReviewDTO dto, CancellationToken token)
        {
            await ValidateUserAndBookExist(dto.UserId , dto.BookId , token);

            var newReview = await _unitOfWork.Reviews.AddAsync(dto.ToBookReview(), token);

            return newReview.ToReviewDTO();
        }



        public async Task<bool> Delete(int Id, CancellationToken token)
        {
            var review = await _unitOfWork.Reviews.GetById(Id , token);
            if (review is null)
                throw new NotFoundException($"No Review exist with id: {Id}");

            return await _unitOfWork.Reviews.DeleteAsync(review , token);
        }



        public async Task<ReviewDTO> EditReview(int Id, AddReviewDTO dto, CancellationToken token)
        {
            await ValidateUserAndBookExist(dto.UserId, dto.BookId, token);

            var review = await _unitOfWork.Reviews.GetById(Id, token);
            if (review is null)
                throw new NotFoundException($"No Review exist with id: {Id}");

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;
            review.CreatedAt = DateTime.Now;

            await _unitOfWork.Reviews.EditAsync(review ,token);

            return review.ToReviewDTO();
        }



        public async Task<List<ReviewDTO>> GetAllForBook(int BookId, CancellationToken token)
        {
            if (await _unitOfWork.Books.GetById(BookId, token) == null)
                throw new NotFoundException($"Book with ID {BookId} was not found.");

            var query = new QuerySpecification<BookReview>();
            query.AddCriteria(x => x.BookId == BookId);

            var reviews = await _unitOfWork.Reviews.GetAll(query, token);

            return reviews.Select(x => x.ToReviewDTO()).ToList();
        }



        public async Task<List<ReviewDTO>> GetAllForUser(string UserId, CancellationToken token)
        {
            if (await _userManager.FindByIdAsync(UserId) == null)
                throw new NotFoundException($"User with ID {UserId} was not found.");

            var query = new QuerySpecification<BookReview>();
            query.AddCriteria(x => x.UserId == UserId);

            var reviews = await _unitOfWork.Reviews.GetAll(query, token);

            return reviews.Select(x => x.ToReviewDTO()).ToList();
        }



        public async Task<ReviewDTO> GetById(int Id, CancellationToken token)
        {
            var review = await _unitOfWork.Reviews.GetById(Id, token);

            if (review == null)
                throw new NotFoundException($"No ReviewExist with ID: {Id}");

            return review.ToReviewDTO();
        }



        private async Task ValidateUserAndBookExist(string userId, int BookId, CancellationToken token) 
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                throw new NotFoundException($"User with ID {userId} was not found.");

            if (await _unitOfWork.Books.GetById(BookId, token) == null)
                throw new NotFoundException($"Book with ID {BookId} was not found.");
        }
    }
}
