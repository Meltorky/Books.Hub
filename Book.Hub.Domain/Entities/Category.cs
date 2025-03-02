using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Domain.Entities
{
    public class Category : BaseEntity
    {
        // Navigation Properties
        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
    }
}
