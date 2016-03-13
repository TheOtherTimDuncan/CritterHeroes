using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Configurations;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Models.LogEvents;
using Microsoft.AspNet.Identity.EntityFramework;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Contexts
{
    [DbConfigurationType(typeof(EntityFrameworkConfiguration))]
    public class BaseDbContext<T> : IdentityDbContext<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>
        where T : DbContext
    {
        private IAppEventPublisher _publisher;

        public BaseDbContext(IAppEventPublisher publisher)
            : this("CritterHeroes", publisher)
        {
        }

        public BaseDbContext(string connectionStringName, IAppEventPublisher publisher)
            : base($"name={connectionStringName}")
        {
            this._publisher = publisher;

            Database.SetInitializer<T>(null);

#if DEBUG
            if (Debugger.IsAttached)
            {
                Debug.WriteLine($"Connection String {connectionStringName}: {Database.Connection.ConnectionString}");
                Database.Log = (string value) => Debug.WriteLine(value);
            }
            else
            {
                Console.WriteLine($"Connection String {connectionStringName}: {Database.Connection.ConnectionString}");
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
                // What is changed needs preserved since the entity states and values will be changed after the save
                IEnumerable<EntityHistory> entityHistories = GetEntityHistories();

                int result = base.SaveChanges();

                // History can't be logged until after save since entity IDs for new entries won't be populated until after save
                LogEntityHistory(entityHistories);

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
                // What is changed needs preserved since the entity states and values will be changed after the save
                IEnumerable<EntityHistory> entityHistories = GetEntityHistories();

                int result = await base.SaveChangesAsync();

                // History can't be logged until after save since entity IDs for new entries won't be populated until after save
                LogEntityHistory(entityHistories);

                return result;
            }
            catch (DbEntityValidationException ex)
            {
                throw new DbEntityValidationDetailException(ex);
            }
        }

        private void LogEntityHistory(IEnumerable<EntityHistory> entityHistories)
        {
            foreach (EntityHistory entityHistory in entityHistories)
            {
                _publisher.Publish(HistoryLogEvent.Create(entityHistory.Entity.ID, entityHistory.Entity.GetType().Name, entityHistory.Before, entityHistory.After));
            }
        }

        private IEnumerable<EntityHistory> GetEntityHistories()
        {
            List<EntityHistory> histories = new List<EntityHistory>();

            IEnumerable<DbEntityEntry<IPreserveHistory>> changedEntries = ChangeTracker.Entries<IPreserveHistory>().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (DbEntityEntry<IPreserveHistory> entry in changedEntries)
            {
                Dictionary<string, object> before;
                if (entry.State == EntityState.Added)
                {
                    before = new Dictionary<string, object>();
                }
                else
                {
                    before = GetPropertyValues(entry.OriginalValues);
                }

                Dictionary<string, object> after = GetPropertyValues(entry.CurrentValues);

                histories.Add(new EntityHistory()
                {
                    Entity = entry.Entity,
                    Before = before,
                    After = after
                });
            }

            return histories;
        }

        private Dictionary<string, object> GetPropertyValues(DbPropertyValues propertyValues)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (string propertyName in propertyValues.PropertyNames)
            {
                object value = propertyValues[propertyName];

                // Complex property values will be of type DbPropertyValues and don't need preserved
                if (!(value is DbPropertyValues))
                {
                    result.Add(propertyName, value);
                }
            }

            return result;
        }

        private class EntityHistory
        {
            public IPreserveHistory Entity
            {
                get;
                set;
            }

            public Dictionary<string, object> Before
            {
                get;
                set;
            }

            public Dictionary<string, object> After
            {
                get;
                set;
            }
        }
    }
}
