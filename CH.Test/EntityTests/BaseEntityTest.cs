using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Contexts;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using TOTD.Utility.ReflectionHelpers;

namespace CH.Test.EntityTests
{
    [TestClass]
    public class BaseEntityTest : BaseTest
    {
        [TestInitialize]
        public void CleanDatabase()
        {
            Mock<IHistoryLogger> mockLogger = new Mock<IHistoryLogger>();

            using (AppUserStorageContext dbContext = new AppUserStorageContext(mockLogger.Object))
            {
                Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AppUserStorageContext>());
                dbContext.Database.Initialize(force: true);

                IEnumerable<string> queries = new[]
                {
                    "BusinessPhone" ,
                    "BusinessGroup",
                    "PersonPhone",
                    "PersonGroup",
                    "Group",
                    "Person",
                    "Business",
                    "AppUserLogin",
                    "AppUserClaim",
                    "AppUser",
                    "AppUserRole",
                    "AppRole",
                    "Critter",
                    "OrganizationSupportedCritter",
                    "Organization",
                    "CritterStatus",
                    "Breed",
                    "Species",
                    "State",
                    "PhoneType"
                }
                    .Select(x => $"DELETE FROM [{x}]");

                string sql = string.Join(";", queries);
                dbContext.Database.ExecuteSqlCommand(sql);
            }
        }

        public async Task VerifyRevisionHistoryIsMaintained<EntityType>(EntityType entity, params string[] ignoreProperties) where EntityType : class, IPreserveHistory
        {
            using (TestSqlStorageContext<EntityType> storageContext = new TestSqlStorageContext<EntityType>())
            {
                storageContext.FillWithTestData(entity, ignoreProperties);
                string jsonAdd = JsonConvert.SerializeObject(GetEntityPropertyValues(entity));

                storageContext.Add(entity);
                await storageContext.SaveChangesAsync();

                storageContext.HistoryBefore.Should().Be("{}");
                storageContext.HistoryAfter.Should().Be(jsonAdd);

                string jsonBefore = JsonConvert.SerializeObject(GetEntityPropertyValues(entity));
                UpdateEntityProperties(entity, ignoreProperties);
                string jsonChanged = JsonConvert.SerializeObject(GetEntityPropertyValues(entity));

                await storageContext.SaveChangesAsync();

                storageContext.HistoryBefore.Should().Be(jsonBefore);
                storageContext.HistoryAfter.Should().Be(jsonChanged);
            }
        }

        private Dictionary<string, object> GetEntityPropertyValues(object entity)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (PropertyInfo property in entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(x => x.CanWrite))
            {
                Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                if (propertyType.IsPrimitive || propertyType == typeof(Decimal) || propertyType == typeof(String) || propertyType == typeof(Guid) || propertyType == typeof(DateTime) || propertyType == typeof(DateTimeOffset))
                {
                    object value = property.GetValue(entity);
                    result.Add(property.Name, value);
                }
            }

            return result;
        }

        private void UpdateEntityProperties(object entity, params string[] ignoreProperties)
        {
            foreach (PropertyInfo property in entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(x => x.CanWrite && !ignoreProperties.Contains(x.Name)))
            {
                Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                object value = null;

                if (propertyType == typeof(string))
                {
                    value = "X";
                }
                else if (propertyType == typeof(DateTimeOffset))
                {
                    value = DateTimeOffset.Now;
                }
                else if (propertyType.IsAssignableFrom(typeof(int)))
                {
                    value = (int)-1;
                }

                if (value != null)
                {
                    property.SetValue(entity, value);
                }
            }
        }
    }
}
