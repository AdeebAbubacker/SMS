using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.Extensions
{
    public static class PaginationExtensions
    {
        /// <summary>
        /// Apply Pagination
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Apply Sorting
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string orderBy, bool? desc = null)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                throw new ArgumentException("The column name for sorting must not be null or empty.", nameof(orderBy));
            }
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression propertyExpression = parameter;
            Type currentType = typeof(T);

            var properties = orderBy.Split('.');
            foreach (var propertyName in properties)
            {
                var propertyInfo = currentType.GetProperties()
                    .FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

                // If not found directly, check for a navigation property
                if (propertyInfo == null)
                {
                    var navProperty = currentType.GetProperties()
                        .FirstOrDefault(p => p.PropertyType.IsClass && p.PropertyType != typeof(string) &&
                                             p.PropertyType.GetProperties()
                                               .Any(subProp => subProp.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)));

                    if (navProperty != null)
                    {
                        propertyExpression = Expression.Property(propertyExpression, navProperty);
                        currentType = navProperty.PropertyType;
                        propertyInfo = currentType.GetProperties()
                            .FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        throw new ArgumentException($"Property '{propertyName}' not found in type '{currentType.Name}'.");
                    }
                }

                if (propertyInfo!.GetCustomAttributes(typeof(NotMappedAttribute), true).Any())
                {
                    throw new ArgumentException($"Cannot sort by '{propertyName}' because it is not mapped to the database.");
                }

                propertyExpression = Expression.Property(propertyExpression, propertyInfo);
                currentType = propertyInfo.PropertyType;
            }

            var convertedExpression = Expression.Convert(propertyExpression, typeof(object));
            var lambda = Expression.Lambda<Func<T, object>>(convertedExpression, parameter);

            return desc == true ? query.OrderByDescending(lambda) : query.OrderBy(lambda);
        }
    }
}
