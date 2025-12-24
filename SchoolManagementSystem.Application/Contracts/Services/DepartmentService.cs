using AutoMapper;
using SchoolManagementSystem.Application.Contracts.IServices;
using SchoolManagementSystem.Application.DTOs.Department;
using SchoolManagementSystem.Application.ViewModels;
using SchoolManagementSystem.Domain.IRepositories;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.Queries;

namespace SchoolManagementSystem.Application.Contracts.Service
{
    public class DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper) : IDepartmentService
    {
        public async Task<DepartmentViewModel> CreateDepartment(CreateDepartmentDTO dto)
        {
            var model = mapper.Map<Department>(dto);
            var department = await departmentRepository.AddAsync(model);
            return mapper.Map<DepartmentViewModel>(department);
        }
        public async Task<PaginatedResponse<DepartmentViewModel>> GetAllDepartments(PaginationFilter filter, DepartmentQuery query,Sort? sort)
        {
            var departments = await departmentRepository.GetAllAsync(filter, query, sort);
            var count = await departmentRepository.CountAsync();

            return new PaginatedResponse<DepartmentViewModel>(mapper.Map<List<DepartmentViewModel>>(departments),
                                                           filter.Page,
                                                           filter.PageSize,
                                                           count);
        }
    }
}