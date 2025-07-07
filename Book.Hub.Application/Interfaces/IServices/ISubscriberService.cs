using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Application.DTOs.Books;
using Books.Hub.Application.Services;
using Books.Hub.Domain.Entities;

namespace Books.Hub.Application.Interfaces.IServices
{
    public interface ISubscriberService
    {
        Task AddAuthorSubscribion( string UserId,int AuthorId, CancellationToken token);
        Task RemoveAuthorSubscribtion( string UserId,int AuthorId, CancellationToken token);
        Task<List<AuthorDTO>> GetSubscribedAuthors( string UserId, CancellationToken token);

        Task BuyBook(string UserId, int BookId, CancellationToken token);
        Task<List<BookDTO>> GetBoughtBooks(string UserId, CancellationToken token);

        Task AddBookToFavourites(string UserId, int BookId, CancellationToken token);
        Task RemoveBookFromFavourites(string UserId, int BookId, CancellationToken token);
        Task<List<BookDTO>> GetFavouriteBooks(string UserId, CancellationToken token);
    }
}
