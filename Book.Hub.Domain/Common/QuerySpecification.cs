using System.Linq.Expressions;

namespace Books.Hub.Domain.Common
{
    public class QuerySpecification<T> where T : class
    {
        public List<Expression<Func<T, bool>>> Criteria { get; } = new();
        public int Skip { get; set; }
        public int Take { get; set; }
        public Expression<Func<T, object>>? OrderBy { get; set; }
        public bool OrderByDescending { get; set; } = false;
        public List<Func<IQueryable<T>, IQueryable<T>>> Includes { get; } = new();

        // helper methods
        public void AddCriteria(Expression<Func<T, bool>> criteria) => Criteria.Add(criteria);
        public void AddInclude(Func<IQueryable<T>, IQueryable<T>> include) => Includes.Add(include);

        //public int pageNumber { get; set; } = 1;
        //public int resultsPerPage { get; set; } = 20;
        //public int Skip => (pageNumber - 1) * resultsPerPage;
        //public int Take => resultsPerPage;
    }
}
