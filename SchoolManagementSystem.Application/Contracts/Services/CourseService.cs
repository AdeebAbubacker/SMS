using AutoMapper;
using SchoolManagementSystem.Application.Contracts.IServices;
using SchoolManagementSystem.Application.DTOs.Course;
using SchoolManagementSystem.Application.Interfaces;
using SchoolManagementSystem.Application.ViewModels;
using SchoolManagementSystem.Domain.IRepositories;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.Queries;

namespace SchoolManagementSystem.Application.Contracts.Service
{
    public class CourseService(ICourseRepository courseRepository, IMapper mapper, ICacheService cacheService) : ICourseService
    {
        string cacheKey = "CourseApiKey";
        public async Task<CourseViewModel> CreateCourse(CreateCourseDto dto)
        {
            var model = mapper.Map<Course>(dto);
            var course = await courseRepository.AddAsync(model);
            // Refresh Cache
            await UpdateCache();
            return mapper.Map<CourseViewModel>(course);
        }
        public async Task<PaginatedResponse<CourseViewModel>> GetAllCourses(PaginationFilter filter, CourseQuery query,Sort? sort)
        {
            var cacheCourses = cacheService.GetData<IEnumerable<Course>>(nameof(Course));
            if (cacheCourses != null )
            {
                var paged = cacheCourses
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToList();

                return new PaginatedResponse<CourseViewModel>(
                    mapper.Map<List<CourseViewModel>>(paged),
                    filter.Page,
                    filter.PageSize,
                    cacheCourses.Count()
                );
            }
            var courses = await courseRepository.GetAllAsync(filter, query, sort);
            var count = await courseRepository.CountAsync();

            return new PaginatedResponse<CourseViewModel>(mapper.Map<List<CourseViewModel>>(courses),
                                                           filter.Page,
                                                           filter.PageSize,
                                                           count);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await courseRepository.DeleteAsync(id);
        }
        public async Task UpdateCache()
        {
            cacheService.RemoveSecrets(nameof(Course));
            var courseList = await courseRepository.GetAllCacheAsync();
            cacheService.SetData(nameof(Course), courseList);
        }
    }
}