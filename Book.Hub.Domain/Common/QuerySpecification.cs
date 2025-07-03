using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Domain.Common
{
    public class QuerySpecification<TEntity>
    {
        public List<Expression<Func<TEntity, bool>>> Criteria { get; } = new();
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public Expression<Func<TEntity, object>>? OrderBy { get; set; }
        public bool? OrderByDescending { get; set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; } = new();

        public void AddInclude(Expression<Func<TEntity, object>> include)
            => Includes.Add(include);

        public void AddCriteria(Expression<Func<TEntity, bool>> criteria)
            => Criteria.Add(criteria);
    }
}
