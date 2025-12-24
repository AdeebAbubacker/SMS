using SchoolManagementSystem.Application.DTOs.Course;
using SchoolManagementSystem.Application.ViewModels;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.Queries;

namespace SchoolManagementSystem.Application.Contracts.IServices
{
    public interface ICourseService
    {
        Task<CourseViewModel> CreateCourse(CreateCourseDto model);
        Task<PaginatedResponse<CourseViewModel>> GetAllCourses(PaginationFilter filter, CourseQuery query, Sort? sort);
        Task<bool> DeleteAsync(Guid id);
        Task UpdateCache();
    }
}