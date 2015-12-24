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

            int skipCount = ((page ?? 1) - 1) * pageSize;

            // Lambda version of Skip/Take will parameterize page values which will improve query plan
            return source.Skip(() => skipCount).Take(() => pageSize);
        }

        public static async Task<IEnumerable<T>> TakePageToListAsync<T>(this IQueryable<T> source, int? page, int pageSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return await source.TakePage(page, pageSize).ToListAsync();
        }

        public static async Task<IEnumerable<TResult>> SelectToListAsync<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return await source.Select(selector).ToListAsync();
        }
    }
}
