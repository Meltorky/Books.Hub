﻿using Books.Hub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(30)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(30)]
        public string LastName { get; set; } = string.Empty;

        // Nav
        public ICollection<AuthorSubscriber> AuthorSubscribers { get; set; } = new Collection<AuthorSubscriber>();
        public ICollection<UserBook> UserBooks { get; set; } = new Collection<UserBook>();
    }
}
