using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CritterHeroes.Web.Data.Contexts;
using CritterHeroes.Web.Data.Models;

namespace CH.DatabaseMigrator.Migrations
{
    public class MigrationsDataContext : AppUserStorageContext
    {
        public virtual IDbSet<Organization> Organizations
        {
            get;
            set;
        }

        public virtual IDbSet<Species> Species
        {
            get;
            set;
        }
    }
}
