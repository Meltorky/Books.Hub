using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
