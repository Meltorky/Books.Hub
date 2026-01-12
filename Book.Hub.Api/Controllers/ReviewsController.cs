using Books.Hub.Application.DTOs.BookReviews;
using Books.Hub.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Books.Hub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }



        /// <summary>
        /// Adds a new book review.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewDTO>> AddReview( [FromBody] AddReviewDTO dto,CancellationToken token)
        {
            var result = await _reviewService.AddReview(dto, token);
            return Ok(result);
        }



        /// <summary>
        /// Deletes a review by its ID.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview(int id, CancellationToken token)
        {
            await _reviewService.Delete(id, token);
            return NoContent();
        }



        /// <summary>
        /// Updates an existing review by ID.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditReview(
            int id,
            [FromBody] AddReviewDTO dto,
            CancellationToken token)
        {
            var result = await _reviewService.EditReview(id, dto, token);
            return Ok(result);
        }



        /// <summary>
        /// Gets all reviews for a specific book.
        /// </summary>
        [HttpGet("book/{bookId}")]
        [ProducesResponseType(typeof(List<ReviewDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllForBook(int bookId, CancellationToken token)
        {
            var result = await _reviewService.GetAllForBook(bookId, token);
            return Ok(result);
        }



        /// <summary>
        /// Gets all reviews written by a specific user.
        /// </summary>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(List<ReviewDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllForUser(string userId, CancellationToken token)
        {
            var result = await _reviewService.GetAllForUser(userId, token);
            return Ok(result);
        }



        /// <summary>
        /// Gets a single review by ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken token)
        {
            var result = await _reviewService.GetById(id, token);
            return Ok(result);
        }
    }
}
