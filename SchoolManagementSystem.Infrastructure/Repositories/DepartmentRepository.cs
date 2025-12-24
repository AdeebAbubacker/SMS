using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Context;
using SchoolManagementSystem.Domain.Extensions;
using SchoolManagementSystem.Domain.IRepositories;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.Queries;

namespace SchoolManagementSystem.Domain.Repositories
{
    public class DepartmentRepository(ApplicationDbContext context) : IDepartmentRepository
    {
        public async Task<Department> AddAsync(Department entity)
        {
            var result = await context.Departments.AddAsync(entity);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<int> CountAsync()
        {
            var count = await context.Departments.CountAsync();

            return count;
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var q = context.Departments;

            return await q.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Department>> GetAllAsync(PaginationFilter filter, DepartmentQuery query, Sort? sort = null)
        {
            return await context.Departments
               .ApplyFilters(query.AsFilters())
               .ApplySorting(sort?.OrderBy ?? "Name", sort?.Descending ?? false)
               .ApplyPagination(filter.Page, filter.PageSize)
               .ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(Guid id)
        {
            return await context.Departments.SingleOrDefaultAsync(al => al.Id == id);
        }

        public async Task<bool> UpdateAsync(Department entity)
        {
            var result = context.Departments.Update(entity);
            await context.SaveChangesAsync();

            return result.State == EntityState.Modified;
        }
    }
}
