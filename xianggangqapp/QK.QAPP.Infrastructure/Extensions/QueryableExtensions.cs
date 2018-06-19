using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string sortBy, string sortDirection)
        {
            string sortingDir = string.Empty;
            string thenByDir = string.Empty;

            if (sortDirection.ToUpper().Trim() == "DESC")
            {
                sortingDir = "OrderByDescending";
            }
            else
            {
                sortingDir = "OrderBy";
            }

            ParameterExpression param = Expression.Parameter(typeof(T), sortBy);
            PropertyInfo pi = typeof(T).GetProperty(sortBy);
            Type[] types = new Type[2];
            types[0] = typeof(T);
            types[1] = pi.PropertyType;


            MethodCallExpression expr = Expression.Call(typeof(Queryable), sortingDir, types, source.Expression, Expression.Lambda(Expression.Property(param, sortBy), param));
            source = source.Provider.CreateQuery<T>(expr);

            return source;
        }
    }
}
