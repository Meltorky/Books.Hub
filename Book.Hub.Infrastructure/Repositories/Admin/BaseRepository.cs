﻿using Books.Hub.Application.Interfaces.IRepositories.Admin;
using Books.Hub.Domain.Entities;
using Books.Hub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Infrastructure.Repositories.Admin
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity?> GetByIdAsync(int Id)
        {
            return await _context.Set<TEntity>().FindAsync(Id);
        }

        //public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeExpressions)
        //{
        //    IQueryable<TEntity> query = _context.Set<TEntity>();

        //    if (includeExpressions.Length > 0) 
        //    {
        //        foreach (var includeExpression in includeExpressions) 
        //        {
        //            query = query.Include(includeExpression);
        //        }
        //    }

        //    return await query
        //        .AsNoTracking()
        //        .ToListAsync();
        //}

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includeExpressions)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includeExpressions.Length > 0) 
            {
                foreach (var includeExpression in includeExpressions)
                {
                    query = includeExpression(query);
                }
            }
         
            return await query.ToListAsync();
        }

    public async Task<TEntity> AddAsync(TEntity model)
        {
            _context.Set<TEntity>().Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> EditAsync(TEntity model)
        {
            _context.Set<TEntity>().Update(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsExitAsync(Expression<Func<TEntity,bool>> predicate)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }
    }
}
