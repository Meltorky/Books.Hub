using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Domain.Entities;

namespace Books.Hub.Application.Mappers
{
    public static class AuthorMappers
    {
        public static AuthorDTO ToAuthorDTO(this Author model) 
        {
            return new AuthorDTO
            {
                Id = model.Id,
                Name = model.Name,
                Bio = model.Bio,
                DateOfBrith = model.DateOfBrith,
                IsActive = model.IsActive,
                Nationality = model.Nationality,
                BooksSold = model.Books.Sum(b => b.TotalCopiesSold),
                SubscribersNumber = model.AuthorSubscribers.Count(),
                ApplicationAuthorId = model.ApplicationAuthorId,
                AuthorImageURL = model.AuthorImageURL,
                Books = model.Books.Select(x => x.ToBookDTO()).ToList()
            };
        }

        public static Author ToAuthor(this AuthorDTO dto)
        {
            return new Author
            {
                Id = dto.Id,
                Name = dto.Name,
                Bio = dto.Bio,
                DateOfBrith = dto.DateOfBrith,
                IsActive = dto.IsActive,
                Nationality = dto.Nationality,
            };
        }
    }
}
