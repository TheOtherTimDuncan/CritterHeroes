using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Configurations;
using CritterHeroes.Web.Data.Models.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Contexts
{
    [DbConfigurationType(typeof(EntityFrameworkConfiguration))]
    public class BaseDbContext : IdentityDbContext<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public BaseDbContext()
            : base("name=CritterHeroes")
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                Database.Log = (string value) => Debug.WriteLine(value);
            }
            else
            {
                Database.Log = Console.WriteLine;
            }
#endif
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            IdentityConfiguration.ConfigureIdentity(modelBuilder);

            modelBuilder.Configurations.Add(new AnimalStatusConfiguration());
            modelBuilder.Configurations.Add(new BreedConfiguration());
            modelBuilder.Configurations.Add(new OrganizationConfiguration());
            modelBuilder.Configurations.Add(new SpeciesConfiguration());

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