using Books.Hub.Application.Identity;

namespace Books.Hub.Domain.Entities
{
    public class AuthorSubscriber
    {
        public int AuthorId { get; set; }
        public string SubscriberId { get; set; } = string.Empty;
        public DateTime SubscribeAt { get; set; } = DateTime.UtcNow;

        public Author Author { get; set; } = null!;
        public ApplicationUser Subscriber { get; set; } = null!;
    }
}
