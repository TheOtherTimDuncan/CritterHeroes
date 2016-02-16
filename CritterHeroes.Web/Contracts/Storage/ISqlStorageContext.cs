using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface ISqlStorageContext<T> : IDisposable where T : class
    {
        IQueryable<T> Entities
        {
            get;
        }

        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();

        void Add(T entity);

        void Delete(T entity);

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
