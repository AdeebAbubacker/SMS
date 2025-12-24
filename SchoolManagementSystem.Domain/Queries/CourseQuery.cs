using SchoolManagementSystem.Domain.Models;

namespace SchoolManagementSystem.Domain.Queries
{
    public class CourseQuery : IQuery<Course>
    {
        public string? Search { get; init; }
        public Guid? DepartmentId { get; init; }

        private CourseQuery()
        {
            // Private constructor to enforce factory methods
        }

        public static CourseQuery FromFilters(
            string? search = null,
            Guid? departmentId = null
        ) => new()
        {
            Search = search,
            DepartmentId = departmentId
        };

        public List<Filter<Course>> AsFilters()
        {
            return [
            // Search across fields
            new(x => DepartmentId == null || DepartmentId == x.DepartmentId),
            new(d =>string.IsNullOrEmpty(Search) || d.Name.ToLower().Contains(Search.ToLower())
                || (d.Description != null && d.Description.ToLower().Contains(Search.ToLower()))
            )];
        }
    }
}
