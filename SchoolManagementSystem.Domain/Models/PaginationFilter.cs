
namespace SchoolManagementSystem.Domain.Models
{
    public class PaginationFilter
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public PaginationFilter()
        {
            Page = 1;
            PageSize = int.MaxValue;
        }
        public PaginationFilter(int page, int pageSize)
        {
            Page = page < 1 ? 1 : page;
            PageSize = pageSize < 1 ? 1 : pageSize;
        }
    }
}
