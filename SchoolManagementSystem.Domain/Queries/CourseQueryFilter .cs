
namespace SchoolManagementSystem.Domain.Queries
{
    public class CourseQueryFilter
    {
        public string? Search { get; set; }
        public Guid? DepartmentId { get; set; }
    }
}