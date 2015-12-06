using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> TakePage<T>(this IQueryable<T> source, int? page, int pageSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Skip(((page ?? 1) - 1) * pageSize).Take(pageSize);
        }

        public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return await source.Select(selector).ToListAsync();
        }
    }
}
