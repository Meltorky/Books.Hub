using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Domain.Common
{
    public class AdvancedSearch
    {
        [MaxLength(100)]
        public string? searchText { get; set; }

        [Range(1,int.MaxValue)]
        [SwaggerSchema(Description = "Page Number start with 1")]
        public int pageNumber { get; set; } = 1;

        [Range(1,100)]
        [SwaggerSchema(Description = "Page size must be between 1 and 100")]
        public int resultsPerPage { get; set; } = 20;
        public string SortedBy { get; set; } = string.Empty;
        public bool IsDesc { get; set; } = false;

        public int Skip => (pageNumber - 1) * resultsPerPage;
        public int Take => resultsPerPage;
    }
}
