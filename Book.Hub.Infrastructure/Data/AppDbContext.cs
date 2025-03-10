﻿using Books.Hub.Application.Comman;
using Books.Hub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Apply OnDelete(DeleteBehavior.Restrict) to all foreign key relationships
            foreach (var forgienKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                forgienKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<BookCategory>()
            .HasKey(b => new { b.BookId, b.CategoryId });


            modelBuilder.Entity<Category>()
            .HasData(new Category[]
            {
                new Category { Id = 1, Name = "Crime" },
                new Category { Id = 2, Name = "Horror" },
                new Category { Id = 3, Name = "Fantasy" },
                new Category { Id = 4, Name = "Romance" },
                new Category { Id = 5, Name = "Mystery" },
                new Category { Id = 6, Name = "Biography" },
                new Category { Id = 7, Name = "Cookbooks" },
                new Category { Id = 8, Name = "Historical" },
                new Category { Id = 9, Name = "Science Fiction" },
                new Category { Id = 10, Name = "Health & Fitness" },
                new Category { Id = 11, Name = "Self-Improvement" },
                new Category { Id = 12, Name = "Business & Finance" }
            });


            // Customize Special Schema for Identity Tables

            modelBuilder.Entity<ApplicationUser>().ToTable("Users", "security");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles", "security");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "security");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "security");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "security");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "security");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "security");
        }
    }
}
