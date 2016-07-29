using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Data.Contexts
{
    public class AppUserCommandStorageContext : BaseDbContext<AppUserCommandStorageContext>, ISqlCommandStorageContext<AppUser>
    {
        public AppUserCommandStorageContext(IAppEventPublisher publisher)
            : base(publisher)
        {
        }

        public AppUserCommandStorageContext(string connectionStringName, IAppEventPublisher publisher)
            : base(connectionStringName, publisher)
        {
        }

        public IQueryable<AppUser> Entities
        {
            get
            {
                return Users;
            }
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
