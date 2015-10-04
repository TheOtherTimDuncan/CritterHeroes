using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> TakePage<T>(this IQueryable<T> source, int page, int pageSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
