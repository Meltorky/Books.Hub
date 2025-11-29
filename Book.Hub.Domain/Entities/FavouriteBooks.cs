using Books.Hub.Application.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Domain.Entities
{
    public class FavouriteBooks
    {
        [Required]
        public string UserId { get; set; } = default!;

        [Required]
        public int BookId { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties
        public ApplicationUser User { get; set; } = null!;
        public Book Book { get; set; } = new();
    }
}
