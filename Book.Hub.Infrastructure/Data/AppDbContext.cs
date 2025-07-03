using Books.Hub.Application.Identity;
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
        public DbSet<UserBook> UserBooks { get; set; }
        public DbSet<AuthorSubscriber> AuthorSubscribers { get; set; }

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

            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookCategories)
                .HasForeignKey(bc => bc.BookId)
                .OnDelete(DeleteBehavior.Cascade); // Delete BookCategory links when Book is deleted

            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BookCategories)
                .HasForeignKey(bc => bc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade); // Delete BookCategory links when Category is deleted



            modelBuilder.Entity<AuthorSubscriber>()
                .HasKey(b => new { b.AuthorId,b.SubscriberId });

            modelBuilder.Entity<AuthorSubscriber>()
                .HasOne(x => x.Author)
                .WithMany(a => a.AuthorSubscribers)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Cascade); // removes only the join row

            modelBuilder.Entity<AuthorSubscriber>()
                .HasOne(x => x.Subscriber)
                .WithMany(s => s.AuthorSubscribers)
                .HasForeignKey(x => x.SubscriberId)
                .OnDelete(DeleteBehavior.Cascade); // removes only the join row



            modelBuilder.Entity<UserBook>()
                 .HasKey(b => new { b.BookId, b.UserId });

            modelBuilder.Entity<UserBook>()
                .HasOne(x => x.Book)
                .WithMany(a => a.UserBooks)
                .HasForeignKey(x => x.BookId)
                .OnDelete(DeleteBehavior.Cascade); // removes only the join row

            modelBuilder.Entity<UserBook>()
                .HasOne(x => x.ApplicationUser)
                .WithMany(s => s.UserBooks)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade); // removes only the join row



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
