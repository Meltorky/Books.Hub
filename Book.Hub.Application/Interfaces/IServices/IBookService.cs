using Books.Hub.Application.DTOs.Books;
using Books.Hub.Domain.Common;

namespace Books.Hub.Application.Interfaces.IServices
{
    public interface IBookService
    {
        Task<BookDTO> GetByIdAsync(int id, CancellationToken token);
        Task<BookDTO> CreateBookAsync(FormBookDTO dto, CancellationToken cancellationToken);

        Task<List<BookDTO>> GetAllAsync(
            AdvancedSearch adv,
            int? categoryId,
            double? minPrice,
            double? maxPrice,
            CancellationToken cancellationToken);

        Task<BookDTO> EditAsync(int Id, FormBookDTO dto, CancellationToken token);

        Task<bool> DeleteAsync(int Id, CancellationToken cancellationToken);
    }
}
