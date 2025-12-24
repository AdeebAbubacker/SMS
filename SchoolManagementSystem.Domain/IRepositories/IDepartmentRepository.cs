using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.Queries;

namespace SchoolManagementSystem.Domain.IRepositories
{
    public interface IDepartmentRepository : IRepository<Department, Guid, DepartmentQuery>
    {
        
    }
}
