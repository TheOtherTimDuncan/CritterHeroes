using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts.Events;
using CritterHeroes.Web.Domain.Contracts.Storage;

namespace CritterHeroes.Web.Data.Contexts
{
    public class SqlCommandStorageContext<T> : BaseDbContext<SqlCommandStorageContext<T>>, ISqlCommandStorageContext<T> where T : class
    {
        public SqlCommandStorageContext(IAppEventPublisher publisher)
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
                return _Entities;
            }
        }

        public virtual void Add(T entity)
        {
            _Entities.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _Entities.Remove(entity);
        }
    }
}
