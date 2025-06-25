using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Hub.Application.Comman;

namespace Books.Hub.Domain.Entities
{
    public class AuthorSubscriber
    {
        public int AuthorId { get; set; }
        public string SubscriberId { get; set; } = string.Empty;
        public Author Author { get; set; } = default!;
        public ApplicationUser Subscriber { get; set; } = default!;
    }
}
