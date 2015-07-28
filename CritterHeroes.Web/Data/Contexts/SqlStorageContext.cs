using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;

namespace CritterHeroes.Web.Data.Contexts
{
    public class SqlStorageContext<T> : BaseDbContext, ISqlStorageContext<T> where T : class
    {
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

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return _Entities.Where(predicate).SingleOrDefault();
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _Entities.Where(predicate).SingleOrDefaultAsync();
        }

        public IEnumerable<T> GetAll()
        {
            return Entities.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Entities.ToListAsync();
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