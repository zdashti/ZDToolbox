using MediatR;

namespace ZDToolbox.Classes
{
    public class PagedQuery<TResponse> : IRequest<TResponse>
    {
        public PagedQuery()
        {
            PageSize = 10;
            PageNumber = 1;
        }

        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public int Skip()
        {
            if (PageNumber < 1) PageNumber = 1;
            return (PageNumber - 1) * PageSize;
        }
    }
}