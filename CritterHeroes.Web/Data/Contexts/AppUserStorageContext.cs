using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Data.Contexts
{
    public class AppUserStorageContext : BaseDbContext, ISqlStorageContext<AppUser>
    {
        public IQueryable<AppUser> Entities
        {
            get
            {
                return Users;
            }
        }

        public virtual AppUser Get(Expression<Func<AppUser, bool>> predicate)
        {
            return Entities.Where(predicate).SingleOrDefault();
        }

        public virtual async Task<AppUser> GetAsync(Expression<Func<AppUser, bool>> predicate)
        {
            return await Entities.Where(predicate).SingleOrDefaultAsync();
        }

        public IEnumerable<AppUser> GetAll()
        {
            return Entities.ToList();
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public virtual void Add(AppUser entity)
        {
            Users.Add(entity);
        }

        public virtual void Delete(AppUser entity)
        {
            Users.Remove(entity);
        }
    }
}