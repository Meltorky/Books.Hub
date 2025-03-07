﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Domain.Entities
{
    public class Author : BaseEntity
    {
        [MaxLength(30)]
        public string Nationality { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Bio { get; set; } = string.Empty ;

        public bool IsActive { get; set; }    // Is This Author still write Books ?

        public DateOnly DateOfBrith { get; set; }

        public byte[]? AuthorImage { get; set; }

        public bool HaveAccount { get; set; } = false;   // Is Author Have an Account in the Syetem

        // Navigation Properties

        public ICollection<Book> Books { get; set; } = new Collection<Book>();

    }
}
