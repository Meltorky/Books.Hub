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
        Task<TEntity?> GetByIdAsync(int Id);    

        //Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeExpressions);
        Task<IEnumerable<TEntity>> GetAllAsync(
            params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includeExpressions);

        Task<TEntity> AddAsync(TEntity entity);

        Task<bool> EditAsync(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);

        Task<bool> IsExitAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
