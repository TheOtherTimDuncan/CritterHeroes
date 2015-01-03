using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface IStorageContext<T> where T : class
    {
        Task<T> GetAsync(string entityID);
        Task<IEnumerable<T>> GetAllAsync();

        Task SaveAsync(T entity);
        Task SaveAsync(IEnumerable<T> entities);

        Task DeleteAsync(T entity);
        Task DeleteAllAsync();
    }
}
