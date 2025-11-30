using Books.Hub.Domain.Common;
using System.Linq.Expressions;

namespace Books.Hub.Application.Interfaces.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetById(int id, CancellationToken token);

        Task<TEntity?> GetById(int id, QuerySpecification<TEntity> spec, CancellationToken token);


        // get range and return List of TEntity
        Task<List<TEntity>> GetRange(List<int> ids, CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions);

        // get range and return List of ids
        Task<List<int>> GetExistingIdsRange(IEnumerable<int> ids, CancellationToken token);
        

        // Dynamic method that replace the 8 overloads below.
        Task<IEnumerable<TEntity>> GetAll
            (QuerySpecification<TEntity> spec, CancellationToken token);
        

        // get all with includes
        Task<IEnumerable<TEntity>> GetAll
            (CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions);


        // get all with pagination then add includes
        Task<IEnumerable<TEntity>> GetAll
            (CancellationToken token, int Skip, int Take, params Expression<Func<TEntity, object>>[] includeExpressions);


        // get all by criteria and add includes
        Task<IEnumerable<TEntity>> GetAll
            (Expression<Func<TEntity, bool>> criteria, CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions);


        // get all by criteria, pagination and add includes
        Task<IEnumerable<TEntity>> GetAll
            (Expression<Func<TEntity, bool>> criteria, int Skip, int Take, CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions);


        // get all with sorting and add includes
        Task<IEnumerable<TEntity>> GetAll
            (Expression<Func<TEntity, object>> OrderBy, bool IsDesc, CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions);


        // get all by criteria then sorting and add includes
        Task<IEnumerable<TEntity>> GetAll
            (Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, object>> OrderBy, bool IsDesc, CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions);


        // get all with pagination then add sorting and add includes
        Task<IEnumerable<TEntity>> GetAll
            (int Skip, int Take, Expression<Func<TEntity, object>> OrderBy, bool IsDesc, CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions);


        // get all by criteria, pagination then add sorting and add includes
        Task<IEnumerable<TEntity>> GetAll
            (Expression<Func<TEntity, bool>> criteria, int Skip, int Take, Expression<Func<TEntity, object>> OrderBy, bool IsDesc, CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions);


        Task<TEntity> AddAsync(TEntity entity, CancellationToken token);

        Task<bool> EditAsync(TEntity entity, CancellationToken token);

        Task<bool> DeleteAsync(TEntity entity, CancellationToken token);

        Task RemoveRange(ICollection<TEntity> entities);

        Task<bool> IsExitAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token);
    }
}
