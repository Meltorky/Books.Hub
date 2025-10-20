using Books.Hub.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Books.Hub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribersController : ControllerBase
    {
        private readonly ISubscriberService _subscriberService;

        public SubscribersController(ISubscriberService subscriberService)
        {
            _subscriberService = subscriberService;
        }



        /// <summary>
        /// Subscribes the user to an author.
        /// </summary>
        [HttpPost("subscribe")]
        public async Task<IActionResult> AddAuthorSubscription(
            [FromQuery] string userId,
            [FromQuery] int authorId,
            CancellationToken token)
        {
            await _subscriberService.AddAuthorSubscribion(userId, authorId, token);
            return NoContent();
        }



        /// <summary>
        /// Removes a user's subscription to an author.
        /// </summary>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpDelete("unsubscribe")]
        public async Task<IActionResult> RemoveAuthorSubscription(
            [FromQuery] string userId,
            [FromQuery] int authorId,
            CancellationToken token)
        {
            await _subscriberService.RemoveAuthorSubscribtion(userId, authorId, token);
            return NoContent();
        }



        /// <summary>
        /// Gets all authors the user is subscribed to.
        /// </summary>
        [HttpGet("{userId}/authors")]
        public async Task<IActionResult> GetSubscribedAuthors([FromRoute] string userId, CancellationToken token)
        {
            var authors = await _subscriberService.GetSubscribedAuthors(userId, token);
            return Ok(authors);
        }



        /// <summary>
        /// Buys a book for the user.
        /// </summary>
        [HttpPost("buy")]
        public async Task<IActionResult> BuyBook(
            [FromQuery] string userId,
            [FromQuery] int bookId,
            CancellationToken token)
        {
            await _subscriberService.BuyBook(userId, bookId, token);
            return NoContent();
        }



        /// <summary>
        /// Gets all books bought by the user.
        /// </summary>
        [HttpGet("{userId}/books")]
        public async Task<IActionResult> GetBoughtBooks([FromRoute] string userId, CancellationToken token)
        {
            var books = await _subscriberService.GetBoughtBooks(userId, token);
            return Ok(books);
        }



        /// <summary>
        /// Adds a book to user's favourites.
        /// </summary>
        [HttpPost("favourite")]
        public async Task<IActionResult> AddBookToFavourites(
            [FromQuery] string userId,
            [FromQuery] int bookId,
            CancellationToken token)
        {
            await _subscriberService.AddBookToFavourites(userId, bookId, token);
            return NoContent();
        }



        /// <summary>
        /// Removes a book from user's favourites.
        /// </summary>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpDelete("favourite")]
        public async Task<IActionResult> RemoveBookFromFavourites(
            [FromQuery] string userId,
            [FromQuery] int bookId,
            CancellationToken token)
        {
            await _subscriberService.RemoveBookFromFavourites(userId, bookId, token);
            return NoContent();
        }



        /// <summary>
        /// Gets all favourite books of the user.
        /// </summary>
        [HttpGet("{userId}/favourites")]
        public async Task<IActionResult> GetFavouriteBooks([FromRoute] string userId, CancellationToken token)
        {
            var books = await _subscriberService.GetFavouriteBooks(userId, token);
            return Ok(books);
        }
    }
}