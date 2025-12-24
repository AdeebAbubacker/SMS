
namespace SchoolManagementSystem.Application.ViewModels
{
    public class PaginatedResponse<T>(List<T> data, int page, int pageSize, int totalRecords)
    {
        public int Page { get; set; } = page;
        public int PageSize { get; set; } = pageSize;
        public int TotalRecords { get; set; } = totalRecords;
        public List<T> Data { get; set; } = data;
    }
}
