using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR.Domain.Contracts
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
