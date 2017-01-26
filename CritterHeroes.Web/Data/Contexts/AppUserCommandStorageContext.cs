using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Domain.Contracts.Events;
using CritterHeroes.Web.Domain.Contracts.Storage;

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
