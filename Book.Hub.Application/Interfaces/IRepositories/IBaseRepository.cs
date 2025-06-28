using Books.Hub.Domain.Constants;
using Books.Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Interfaces.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetById(int Id, CancellationToken token);

        Task<TEntity?> GetById(int Id, CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions);

        Task<TEntity?> FindSingleByCriteria(Expression<Func<TEntity, bool>> criteria, CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions);
        
        Task<IEnumerable<TEntity>> GetRange(List<int> ids, CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions);


        // get all with includes
        Task<IEnumerable<TEntity>> GetAll
            (CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions);


        // get all with pagination then add includes
        Task<IEnumerable<TEntity>> GetAll
            (int Skip, int Take, CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions);


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

        Task<bool> IsExitAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token);
    }
}
