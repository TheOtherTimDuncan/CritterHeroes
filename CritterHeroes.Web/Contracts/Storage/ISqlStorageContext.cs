using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface ISqlStorageContext<T> where T : class
    {
        IQueryable<T> Entities
        {
            get;
        }

        T Get(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
