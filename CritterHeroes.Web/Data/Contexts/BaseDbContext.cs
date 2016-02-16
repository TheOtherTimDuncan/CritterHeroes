using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Data.Configurations;
using CritterHeroes.Web.Data.Models.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Contexts
{
    [DbConfigurationType(typeof(EntityFrameworkConfiguration))]
    public class BaseDbContext<T> : IdentityDbContext<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>
        where T : DbContext
    {
        private IHistoryLogger _logger;

        public BaseDbContext(IHistoryLogger logger)
            : base("name=CritterHeroes")
        {
            this._logger = logger;

            Database.SetInitializer<T>(null);

#if DEBUG
            if (Debugger.IsAttached)
            {
                Debug.WriteLine(Database.Connection.ConnectionString);
                Database.Log = (string value) => Debug.WriteLine(value);
            }
            else
            {
                Console.WriteLine(Database.Connection.ConnectionString);
                Database.Log = Console.WriteLine;
            }
#endif
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            IdentityConfiguration.ConfigureIdentity(modelBuilder);

            modelBuilder.Configurations.AddFromAssembly(typeof(CritterConfiguration).Assembly);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public override int SaveChanges()
        {
            try
            {
                int result = base.SaveChanges();
                return result;
            }
            catch (DbEntityValidationException ex)
            {
                throw new DbEntityValidationDetailException(ex);
            }
        }

        public override async Task<int> SaveChangesAsync()
        {
            try
            {
                int result = await base.SaveChangesAsync();
                return result;
            }
            catch (DbEntityValidationException ex)
            {
                throw new DbEntityValidationDetailException(ex);
            }
        }
    }
}