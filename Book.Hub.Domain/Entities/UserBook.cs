using Books.Hub.Application.Identity;

namespace Books.Hub.Domain.Entities
{
    public class UserBook
    {
        public int BookId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public Book Book { get; set; } = default!;
        public ApplicationUser ApplicationUser { get; set; } = default!;
    }
}
