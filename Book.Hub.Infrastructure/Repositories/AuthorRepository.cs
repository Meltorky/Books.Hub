using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Domain.Entities;
using Books.Hub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Books.Hub.Application.DTOs.Books;
using Books.Hub.Application.Mappers;

namespace Books.Hub.Infrastructure.Repositories
{
    public class AuthorRepository : BaseRepository<Author> , IAuthorRepository
    {
        private readonly AppDbContext _context;
        public AuthorRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<AuthorDTO?> GetByIdAsyncOptimized(int id, CancellationToken token)
        {
            var dto = await _context.Authors
                .Where(a => a.Id == id)
                .Select(a => new AuthorDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Nationality = a.Nationality,
                    Bio = a.Bio,
                    DateOfBrith = a.DateOfBrith,
                    ApplicationAuthorId = a.ApplicationAuthorId,
                    AuthorImageURL = a.AuthorImageURL,
                    IsActive = a.IsActive,
                    BooksSold = a.Books.Sum(b => b.TotalCopiesSold),
                    SubscribersNumber = a.AuthorSubscribers.Count(),
                    Books = a.Books.Select(i => i.ToBookDTO(a.Name)).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(token);

            return dto;
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
