using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Domain.Contracts.Storage
{
    public interface ISqlCommandStorageContext<T> : IDisposable where T : class
    {
        IQueryable<T> Entities
        {
            get;
        }

        void Add(T entity);

        void Delete(T entity);

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
