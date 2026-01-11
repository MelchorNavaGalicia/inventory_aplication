using Microsoft.AspNetCore.Http.Features;

namespace inventory_aplication.Application.Features.Common.Results
{
    public class PagedResult<T>
    {
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; } = [];

        public PagedResult( int totalItems, int pageNumber, int pageSize, List<T> items)
        {
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Items = items;
        }
        //public static PagedResult<T> Ok(int TotalItems, int Page , int PageSize, List<T> Items) =>
        //    new PagedResult<T> { TotalItems = TotalItems, Page = Page, PageSize = PageSize , Items = Items };
    }
}
