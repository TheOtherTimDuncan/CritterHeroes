using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Configurations;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Contexts
{
    [DbConfigurationType(typeof(EntityFrameworkConfiguration))]
    public class BaseDbContext : DbContext
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

            modelBuilder.Configurations.Add(new AnimalStatusConfiguration());

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

    public abstract class BaseDbContext<T> : BaseDbContext, IMasterStorageContext<T> where T : class
    {
        public virtual IDbSet<T> _Items
        {
            get;
            set;
        }

        public IQueryable<T> Items
        {
            get
            {
                return _Items;
            }
        }

        public abstract Task<T> GetAsync(string entityID);

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Items.ToListAsync();
        }

        public virtual void Add(T entity)
        {
            _Items.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _Items.Remove(entity);
        }

        public Task SaveAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}