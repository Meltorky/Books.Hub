﻿using System.Linq.Expressions;
using Books.Hub.Application.Common.Exceptions;
using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Application.DTOs.Books;
using Books.Hub.Application.Identity;
using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Application.Mappers;
using Books.Hub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Books.Hub.Application.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public SubscriberService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }



        public async Task AddAuthorSubscribion(string userId, int AuthorId, CancellationToken token)
        {
            var user = await IsUserExist(userId, token , x => x.AuthorSubscribers);

            if (await _unitOfWork.Authors.GetById(AuthorId, token) is null)
                throw new NotFoundException($"Author with ID {userId} was not found.");


            if (user.AuthorSubscribers.Any(x => x.AuthorId == AuthorId))
                throw new ConflictException("Already subscribed to this author.");

            user.AuthorSubscribers.Add(new AuthorSubscriber
            {
                AuthorId = AuthorId,
                SubscriberId = userId
            });

            await _unitOfWork.SaveChangesAsync(token);
        }



        public async Task RemoveAuthorSubscribtion(string UserId, int AuthorId, CancellationToken token)
        {
            var user = await IsUserExist(UserId, token, x => x.AuthorSubscribers);

            var subscription = user.AuthorSubscribers.SingleOrDefault(x => x.AuthorId == AuthorId);

            if (subscription is null)
                throw new NotFoundException("Subscription does not exist.");

            user.AuthorSubscribers.Remove(subscription);

            await _unitOfWork.SaveChangesAsync(token);
        }



        public async Task<List<AuthorDTO>> GetSubscribedAuthors(string UserId, CancellationToken token)
        {
            var user = await IsUserExist(UserId, token, x => x.AuthorSubscribers);

            var authors = user.AuthorSubscribers.Select(x => x.Author).ToList();

            return authors.Select(a => a.ToAuthorDTO()).ToList();
        }



        public async Task BuyBook(string UserId, int BookId, CancellationToken token)
        {
            var user = await IsUserExist(UserId, token, x => x.UserBooks);

            if (await _unitOfWork.Books.GetById(BookId, token) is null)
                throw new NotFoundException($"Book with ID {BookId} was not found.");


            if (user.UserBooks.Any(x => x.BookId == BookId))
                throw new ConflictException("You had Bought this Book Before.");

            user.UserBooks.Add(new UserBook { UserId = UserId, BookId = BookId });

            await _unitOfWork.SaveChangesAsync(token);
        }



        public async Task<List<BookDTO>> GetBoughtBooks(string UserId, CancellationToken token)
        {
            var user = await IsUserExist(UserId , token, x => x.UserBooks);

            var books = user.UserBooks.Select(x => x.Book).ToList();

            return books.Select(x => x.ToBookDTO()).ToList();
        }



        public async Task AddBookToFavourites(string UserId, int BookId, CancellationToken token)
        {
            var user = await IsUserExist(UserId, token);

            if (await _unitOfWork.Books.GetById(BookId, token) != null && !user.FavouriteBooks.Contains(BookId))
            {
                user.FavouriteBooks.Add(BookId);
            }
            else
            {
                throw new ArgumentException(); 
            }

            await _unitOfWork.SaveChangesAsync(token);
        }



        public async Task RemoveBookFromFavourites(string UserId, int BookId, CancellationToken token)
        {
            var user = await IsUserExist(UserId, token);

            if (await _unitOfWork.Books.GetById(BookId, token) != null && user.FavouriteBooks.Contains(BookId)) 
            {
                user.FavouriteBooks.Remove(BookId);
            }
            else
            {
                throw new ArgumentException();
            }

            await _unitOfWork.SaveChangesAsync(token);
        }



        public async Task<List<BookDTO>> GetFavouriteBooks(string UserId, CancellationToken token)
        {
            var user = await IsUserExist(UserId, token);
            var bookIDs = user.FavouriteBooks.ToList();

            var books = await _unitOfWork.Books.GetRange( bookIDs, token);
            return books.Select(x => x.ToBookDTO()).ToList();
        }



        private async Task<ApplicationUser> IsUserExist(string UserId
            , CancellationToken token, params Expression<Func<ApplicationUser, object>>[] includes) 
        {
            var query = _userManager.Users.AsQueryable();

            if (includes.Any())
                foreach (var include in includes)
                    query = query.Include(include);

            var user = await query.FirstOrDefaultAsync(x => x.Id == UserId);
            if (user is null)
                throw new NotFoundException($"User with ID {UserId} was not found.");

            return user;
        }

    }
}
