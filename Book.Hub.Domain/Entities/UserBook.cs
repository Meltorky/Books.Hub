using Books.Hub.Application.Identity;
using System.ComponentModel.DataAnnotations;

namespace Books.Hub.Domain.Entities
{
    public class UserBook  // user buy book
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public DateTime SoldAt { get; set; } = DateTime.UtcNow;

        // nav prop
        public Book Book { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
