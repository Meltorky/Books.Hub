
namespace Books.Hub.Domain.Entities
{
    public class Category : BaseEntity
    {
        // Navigation Properties
        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
    }
}
