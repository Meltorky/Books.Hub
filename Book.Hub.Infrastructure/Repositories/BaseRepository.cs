using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Domain.Common;
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



        // Get specific Entity with Id for edit and Delete
        public async Task<TEntity?> GetByIdFast(int id, CancellationToken token)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }



        // Get specific Entity with Id with includes
        public async Task<TEntity?> GetById(int id, CancellationToken token, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes is not null && includes.Any())
                foreach (var include in includes)
                    query = include(query);

            return await query
                .AsNoTracking()
                .SingleOrDefaultAsync(e => EF.Property<int>(e, "Id") == id, token);
        }



        // Get specific Entity with Id
        public async Task<TEntity?> GetById(int id, QuerySpecification<TEntity> spec, CancellationToken token)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (spec.Includes.Any())
                foreach (var include in spec.Includes)
                    query = include(query);

            return await query
                .AsNoTracking()
                .SingleOrDefaultAsync(e => EF.Property<int>(e, "Id") == id, token);
        }



        // get range and return List of TEntity
        public async Task<List<TEntity>> GetRange(List<int> ids
            , CancellationToken token, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            if (ids == null || !ids.Any())
                return Enumerable.Empty<TEntity>().ToList();

            IQueryable<TEntity> query = _context.Set<TEntity>()
                .Where(e => ids.Contains(EF.Property<int>(e, "Id")));

            if (includeExpressions.Any())
                foreach (var include in includeExpressions)
                query = query.Include(include);

            return await query.ToListAsync(token);
        }


        // get range and return List of ids VVV
        public async Task<List<int>> GetExistingIdsRange(IEnumerable<int> ids,CancellationToken token)
        {
            if (ids == null || !ids.Any())
                return new List<int>();

            return await _context.Set<TEntity>()
                .Where(e => ids.Contains(EF.Property<int>(e,"Id")))
                .Select(e => EF.Property<int>(e, "Id"))
                .ToListAsync(token);
        }



        // Dynamic method that replace the 8 overloads below.
        public async Task<IEnumerable<TEntity>> GetAll(QuerySpecification<TEntity> spec,CancellationToken token)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            // Apply filtering
            foreach (var criteria in spec.Criteria)
                query = query.Where(criteria);

            // Apply .Include().ThenInclude() chains
            foreach (var include in spec.Includes)
                query = include(query);

            // Apply sorting
            if (spec.OrderBy != null)
            {
                query = spec.OrderByDescending == true
                    ? query.OrderByDescending(spec.OrderBy)
                    : query.OrderBy(spec.OrderBy);
            }

            // Apply pagination
            query = query.Skip(spec.Skip);
            query = query.Take(spec.Take);

            return await query.AsNoTracking().ToListAsync(token);
        }

        public async Task<TEntity> AddAsync(TEntity model, CancellationToken token)
        {
            _context.Set<TEntity>().Add(model);
            await _context.SaveChangesAsync(token);
            return model;
        }



        public async Task<TEntity> EditAsync(TEntity model, CancellationToken token)
        {
            _context.Set<TEntity>().Update(model);
            await _context.SaveChangesAsync(token);
            return model;
        }



        public async Task<bool> DeleteAsync(TEntity entity, CancellationToken token)
        {
            _context.Set<TEntity>().Remove(entity);
            return await _context.SaveChangesAsync(token) > 0;
        }



        public async Task RemoveRange(ICollection<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }



        public async Task<bool> IsExitAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate, token);
        }








        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="includeExpressions"></param>
        /// <returns></returns>










        // get all with includes VVV
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
        public async Task<IEnumerable<TEntity>> GetAll(CancellationToken token, int Skip = 0, int Take = 10
            , params Expression<Func<TEntity, object>>[] includeExpressions)
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
            , Expression<Func<TEntity, object>> OrderBy, bool IsDesc
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
    }
}
