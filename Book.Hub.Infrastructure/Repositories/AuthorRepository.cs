using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Domain.Entities;
using Books.Hub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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
    }
}
