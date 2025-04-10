namespace ECommerce.Core.Sharing
{
    public class Pagination<T> where T : class
    {
        public Pagination(int pageNumber, int pageSize, int totalCount, IEnumerable<T> data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            Data = data;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        public IEnumerable<T> Data { get; set; }
    }
}
