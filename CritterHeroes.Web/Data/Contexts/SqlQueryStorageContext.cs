using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Domain.Contracts.Events;
using CritterHeroes.Web.Domain.Contracts.Storage;

namespace CritterHeroes.Web.Data.Contexts
{
    public class SqlQueryStorageContext<T> : BaseDbContext<SqlQueryStorageContext<T>>, ISqlQueryStorageContext<T> where T : class
    {
        public SqlQueryStorageContext(IAppEventPublisher publisher)
            : base(publisher)
        {
        }

        public virtual IDbSet<T> _Entities
        {
            get;
            set;
        }

        public IQueryable<T> Entities
        {
            get
            {
                return _Entities.AsNoTracking();
            }
        }

        public IEnumerable<T> GetAll()
        {
            return Entities.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }
    }
}
