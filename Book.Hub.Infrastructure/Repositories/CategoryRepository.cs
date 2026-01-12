using Books.Hub.Application.DTOs.Categories;
using Books.Hub.Application.Interfaces.IRepositories;
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
    public class CategoryRepository : BaseRepository<Category> , ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CategoryDTO>> GetAllAsync(CancellationToken token)
        {
            return await _context.Categories
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    BooksNumber = c.BookCategories.Count()
                })
                .OrderByDescending(x => x.BooksNumber)
                .AsNoTracking()
                .ToListAsync(token);
        }
    }
}
