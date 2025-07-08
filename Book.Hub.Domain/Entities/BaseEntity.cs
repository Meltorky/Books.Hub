using System.ComponentModel.DataAnnotations;

namespace Books.Hub.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(80)]
        public string Name { get; set; } = string.Empty;
    }
}
