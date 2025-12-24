
namespace SchoolManagementSystem.Domain.Queries
{
    public interface IQuery<T>
    {
        public List<Filter<T>> AsFilters();
    }
}