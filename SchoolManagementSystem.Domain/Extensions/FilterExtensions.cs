using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Domain.Extensions
{
    public static class FilterExtensions
    {
        /// <summary>
        /// Apply Filters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, IEnumerable<Filter<T>> filters)
        {
            foreach (var filter in filters)
            {
                query = query.Where(filter.Criteria);
            }
            return query;
        }
    }
}