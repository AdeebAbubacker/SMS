using SchoolManagementSystem.Application.DTOs.Department;
using SchoolManagementSystem.Application.ViewModels;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.Queries;

namespace SchoolManagementSystem.Application.Contracts.IServices
{
    public interface IDepartmentService
    {
        Task<DepartmentViewModel> CreateDepartment(CreateDepartmentDTO model);
        Task<PaginatedResponse<DepartmentViewModel>> GetAllDepartments(PaginationFilter filter, DepartmentQuery query, Sort? sort);
    }
}