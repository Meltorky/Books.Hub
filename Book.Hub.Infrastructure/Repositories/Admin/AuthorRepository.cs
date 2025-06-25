using Books.Hub.Application.Interfaces.IRepositories.Admin;
using Books.Hub.Domain.Entities;
using Books.Hub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Infrastructure.Repositories.Admin
{
    public class AuthorRepository : BaseRepository<Author> , IAuthorRepository
    {
        private readonly AppDbContext _context;
        public AuthorRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<Author>> GetAllAsync() 
        //{
        //    return await _context.Authors
        //        .AsNoTracking()
        //        .OrderBy(x => x.Name)
        //        .ToListAsync();
        //}

        //public async Task<Author?> GetByIdAsync(int Id) 
        //{
        //    return await _context.Authors.FindAsync(Id);
        //}

        //public async Task<Author> AddAsync(Author model) 
        //{
        //    _context.Authors.Add(model);
        //    await _context.SaveChangesAsync();
        //    return model;
        //}

        //public async Task<bool> EditAsync(Author model) 
        //{
        //    _context.Authors.Update(model);
        //    return await _context.SaveChangesAsync() > 0;
        //}

        //public async Task<bool> IsExit(int Id)
        //{
        //    return await _context.Authors.AnyAsync(a => a.Id == Id);
        //}

        //public async Task<bool> DeleteAsync(Author author) 
        //{
        //    _context.Authors.Remove(author);
        //    return await _context.SaveChangesAsync() > 0;
        //}
    }
}
