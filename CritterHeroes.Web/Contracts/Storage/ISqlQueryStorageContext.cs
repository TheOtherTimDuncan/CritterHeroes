using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface ISqlQueryStorageContext<T> : IDisposable where T : class
    {
        IQueryable<T> Entities
        {
            get;
        }

        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
    }
}
