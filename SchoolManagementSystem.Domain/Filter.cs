using System.Linq.Expressions;

namespace SchoolManagementSystem.Domain
{
    public class Filter<T>
    {
        public Expression<Func<T, bool>> Criteria { get; }

        public Filter(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
    }
}