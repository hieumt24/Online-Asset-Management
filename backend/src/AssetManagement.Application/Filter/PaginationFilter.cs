namespace AssetManagement.Application.Filter
{
    public class PaginationFilter
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public PaginationFilter()
        {
            this.PageIndex = 1;
            this.PageSize = 10;
        }

        public PaginationFilter(int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex < 1 ? 1 : pageIndex;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}