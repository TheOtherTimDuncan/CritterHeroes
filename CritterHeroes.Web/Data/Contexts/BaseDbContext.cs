using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Configurations;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Contexts
{
    [DbConfigurationType(typeof(EntityFrameworkConfiguration))]
    public class BaseDbContext : DbContext
    {

        public BaseDbContext(string connectionStringName)
            : base("name=" + connectionStringName)
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