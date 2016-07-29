using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Data.Contexts
{
    public class AppUserQueryStorageContext : BaseDbContext<AppUserQueryStorageContext>, ISqlQueryStorageContext<AppUser>
    {
        public AppUserQueryStorageContext(IAppEventPublisher publisher)
            : base(publisher)
        {
        }

        public IQueryable<AppUser> Entities
        {
            get
            {
                return Users.AsNoTracking();
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
    }
}
