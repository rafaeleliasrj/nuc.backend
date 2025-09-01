using Avvo.Core.Commons.Pagination;

namespace Avvo.Core.Data.Pagination
{
    public class PagedResult<TEntity> : PagedResultBase where TEntity : class
    {
        public IEnumerable<TEntity> Results { get; set; }
        public PagedResult()
        {
            Results = new List<TEntity>();
        }
    }
}
