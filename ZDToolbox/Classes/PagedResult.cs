using MediatR;

namespace ZDToolbox.Classes
{
    public class PagedResult<TResponse> : ApiResult, IRequest<TResponse>
    {
        public PagedMetaData PagedMetaData { get; set; }
        public List<TResponse> Results { get; set; }

        public PagedResult()
        {

        }
        public PagedResult(IEnumerable<TResponse> items, int count, int pageNumber, int pageSize)
        {
            PagedMetaData = new PagedMetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            Results = new List<TResponse>();
            Results.AddRange(items);
        }
        /*
         public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }s
         
         */
    }
}
