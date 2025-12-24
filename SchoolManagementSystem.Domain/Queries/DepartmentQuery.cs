using SchoolManagementSystem.Domain.Models;

namespace SchoolManagementSystem.Domain.Queries
{
    public class DepartmentQuery : IQuery<Department>
    {
        public string? Search { get; init; }

        private DepartmentQuery()
        {
            // Private constructor to enforce factory methods
        }

        public static DepartmentQuery FromFilters(
            string? search = null
        ) => new()
        {
            Search = search
        };

        public List<Filter<Department>> AsFilters()
        {
            return [
            // Search across fields
            new(d =>string.IsNullOrEmpty(Search) || d.Name.ToLower().Contains(Search.ToLower())
                || (d.Description != null && d.Description.ToLower().Contains(Search.ToLower()))
            ) ];
        }
    }
}
