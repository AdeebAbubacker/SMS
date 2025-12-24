
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.Queries;

namespace SchoolManagementSystem.Domain.IRepositories
{
    public interface ICourseRepository :  IRepository<Course, Guid, CourseQuery>
    {
        Task<IEnumerable<Course>> GetAllCacheAsync();
    }
}