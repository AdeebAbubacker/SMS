using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Context;
using SchoolManagementSystem.Domain.Extensions;
using SchoolManagementSystem.Domain.IRepositories;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.Queries;

namespace SchoolManagementSystem.Infrastructure.Repositories
{
    public class CourseRepository(ApplicationDbContext context) : ICourseRepository
    {
        public async Task<Course> AddAsync(Course entity)
        {
            var result = await context.Courses.AddAsync(entity);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<int> CountAsync()
        {
            var count = await context.Courses.CountAsync();

            return count;
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var q = context.Courses;

            return await q.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Course>> GetAllAsync(PaginationFilter filter, CourseQuery query, Sort? sort = null)
        {
            return await context.Courses
               .ApplyFilters(query.AsFilters())
               .ApplySorting(sort?.OrderBy ?? "Name", sort?.Descending ?? false)
               .ApplyPagination(filter.Page, filter.PageSize)
               .ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(Guid id)
        {
            return await context.Courses.SingleOrDefaultAsync(al => al.Id == id);
        }

        public async Task<bool> UpdateAsync(Course entity)
        {
            var result = context.Courses.Update(entity);
            await context.SaveChangesAsync();

            return result.State == EntityState.Modified;
        }

        public async Task<IEnumerable<Course>> GetAllCacheAsync()
        {
            return await context.Courses.ToListAsync();
        }
    }
}