using SchoolManagementSystem.Domain.Models;

namespace SchoolManagementSystem.Domain.IRepositories
{
    public interface IRepository<TModel, TKey, TQuery>
    {
        public Task<bool> DeleteAsync(TKey id);
        public Task<bool> ExistsAsync(TKey id);
        public Task<int> CountAsync();
        public Task<IEnumerable<TModel>> GetAllAsync(PaginationFilter filter, TQuery query, Sort? sort = null);
        public Task<bool> UpdateAsync(TModel entity);
        public Task<TModel?> GetByIdAsync(TKey id); 
        public Task<TModel> AddAsync(TModel entity); 
    }
}