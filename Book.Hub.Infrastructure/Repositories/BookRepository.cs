﻿using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Domain.Entities;
using Books.Hub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Infrastructure.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly AppDbContext _context;
        public BookRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


        // EF Does Not Track them, Used For Editing an existing Book  VVV
        public async Task RemoveBookCategories(ICollection<BookCategory> bookCategories)
        {
            _context.BookCategories.RemoveRange(bookCategories);
            await _context.SaveChangesAsync();
        }



        // EF Core Tracks them, Used For Deleting an existing Book
        //public async Task RemoveBookCategories(Book book)
        //{
        //    var relatedCategories = await _context.BookCategories
        //        .Where(bc => bc.BookId == book.Id)
        //        .ToListAsync(); // Ensure EF Core tracks them

        //    if (relatedCategories.Any())
        //        _context.BookCategories.RemoveRange(relatedCategories);

        //    await _context.SaveChangesAsync();
        //}

    }
}
