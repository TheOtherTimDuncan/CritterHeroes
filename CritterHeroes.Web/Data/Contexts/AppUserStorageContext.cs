using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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