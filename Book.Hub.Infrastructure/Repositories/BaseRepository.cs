using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Books.Hub.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<TEntity?> GetById(int Id, CancellationToken token) 
        {
            return await _context.Set<TEntity>().FindAsync(Id,token);
        }


        public async Task<TEntity?> GetById(int Id
            , CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includeExpressions.Any())
                foreach (var includeExpression in includeExpressions)
                    query.Include(includeExpression);

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e,"Id") == Id,token);
        }


        public async Task<TEntity?> FindSingleByCriteria(Expression<Func<TEntity, bool>> criteria
            , CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includeExpressions.Any())
                foreach (var includeExpression in includeExpressions)
                    query.Include(includeExpression);

            return await query.SingleOrDefaultAsync(criteria,token);     
        }


        public async Task<IEnumerable<TEntity>> GetRange(List<int> ids
            , CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            if (ids == null || !ids.Any())
                return Enumerable.Empty<TEntity>();

            IQueryable<TEntity> query = _context.Set<TEntity>()
                .Where(e => ids.Contains(EF.Property<int>(e, "Id")));

            if (includeExpressions.Any())
                foreach (var include in includeExpressions)
                query = query.Include(include);

            return await query.ToListAsync(token);
        }


        // get all with includes
        public async Task<IEnumerable<TEntity>> GetAll(
            CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includeExpressions.Any())
                foreach (var includeExpression in includeExpressions)
                    query.Include(includeExpression);

            return await query.ToListAsync(token);
        }


        // get all with pagination and add includes
        public async Task<IEnumerable<TEntity>> GetAll(int Skip, int Take
            , CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>()
                .Skip(Skip)
                .Take(Take);

            if (includeExpressions.Any())
                foreach (var includeExpression in includeExpressions)
                    query.Include(includeExpression);

            return await query.ToListAsync(token);
        }


        // get all by criteria and add includes
        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> criteria
            , CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>()
                .Where(criteria);

            if (includeExpressions.Any())
                foreach (var includeExpression in includeExpressions)
                    query.Include(includeExpression);

            return await query.ToListAsync(token);
        }


        // get all by criteria, pagination and add includes
        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> criteria, int Skip, int Take
            , CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>()
                .Where(criteria)
                .Skip(Skip)
                .Take(Take);

            if (includeExpressions.Any())
                foreach (var includeExpression in includeExpressions)   
                    query.Include(includeExpression);

            return await query.ToListAsync(token);
        }


        // get all with sorting and add includes
        public async Task<IEnumerable<TEntity>> GetAll
            (Expression<Func<TEntity, object>> OrderBy, bool IsDesc, CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions)
        
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (IsDesc)
                query.OrderByDescending(OrderBy);
            else
                query.OrderBy(OrderBy);

            if (includeExpressions.Any())
                foreach (var includeExpression in includeExpressions)
                    query.Include(includeExpression);

            return await query.ToListAsync(token);
        }


        // get all by criteria then sorting and add includes
        public async Task<IEnumerable<TEntity>> GetAll
            (Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, object>> OrderBy, bool IsDesc, CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>()
                .Where(criteria);

            if (IsDesc)
                query.OrderByDescending(OrderBy);
            else
                query.OrderBy(OrderBy);

            if (includeExpressions.Any())
                foreach (var includeExpression in includeExpressions)
                    query.Include(includeExpression);

            return await query.ToListAsync(token);
        }


        // get all with pagination then add sorting and add includes
        public async Task<IEnumerable<TEntity>> GetAll
            (int Skip, int Take, Expression<Func<TEntity, object>> OrderBy, bool IsDesc, CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (IsDesc)
                query.OrderByDescending(OrderBy)
                    .Skip(Skip)
                    .Take(Take);
            else
                query.OrderBy(OrderBy)
                    .Skip(Skip)
                    .Take(Take);

            if (includeExpressions.Any())
                foreach (var includeExpression in includeExpressions)
                    query.Include(includeExpression);

            return await query.ToListAsync(token);
        }


        // get all by criteria, pagination then add sorting and add includes
        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> criteria, int Skip, int Take
            , Expression<Func<TEntity, object>> OrderBy ,bool IsDesc
            , CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>()
                .Where(criteria);

            if (IsDesc) 
                query.OrderByDescending(OrderBy)
                    .Skip(Skip)
                    .Take(Take);
            else
                query.OrderBy(OrderBy)
                    .Skip(Skip)
                    .Take(Take);

            if (includeExpressions.Any())
                foreach (var includeExpression in includeExpressions)
                    query.Include(includeExpression);

            return await query.ToListAsync(token);
        }


        public async Task<TEntity> AddAsync(TEntity model, CancellationToken token)
        {
            _context.Set<TEntity>().Add(model);
            await _context.SaveChangesAsync(token);
            return model;
        }


        public async Task<bool> EditAsync(TEntity model, CancellationToken token)
        {
            _context.Set<TEntity>().Update(model);
            return await _context.SaveChangesAsync(token) > 0;
        }


        public async Task<bool> DeleteAsync(TEntity entity, CancellationToken token)
        {
            _context.Set<TEntity>().Remove(entity);
            return await _context.SaveChangesAsync(token) > 0;
        }


        public async Task<bool> IsExitAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate, token);
        }

    }
}
