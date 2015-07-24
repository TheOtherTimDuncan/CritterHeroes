using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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

    public abstract class BaseDbContext<T> : BaseDbContext, ISqlStorageContext<T> where T : class
    {
        public virtual IDbSet<T> _Entities
        {
            get;
            set;
        }

        public IQueryable<T> Entities
        {
            get
            {
                return _Entities;
            }
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return _Entities.Where(predicate).SingleOrDefault();
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _Entities.Where(predicate).SingleOrDefaultAsync();
        }

        public IEnumerable<T> GetAll()
        {
            return Entities.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public virtual void Add(T entity)
        {
            _Entities.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _Entities.Remove(entity);
        }
    }
}