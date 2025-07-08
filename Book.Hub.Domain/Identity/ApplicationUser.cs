using Books.Hub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Books.Hub.Application.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(30)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(30)]
        public string LastName { get; set; } = string.Empty;

        public List<int> FavouriteBooks { get; set; } = new List<int>();

        // Nav
        public ICollection<AuthorSubscriber> AuthorSubscribers { get; set; } = new Collection<AuthorSubscriber>();
        public ICollection<UserBook> UserBooks { get; set; } = new Collection<UserBook>();
        public ICollection<BookReview> BookReviews { get; set; } = new List<BookReview>();
    }
}
