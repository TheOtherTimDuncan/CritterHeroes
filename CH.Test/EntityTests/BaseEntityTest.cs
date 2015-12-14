using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using CritterHeroes.Web.Data.Contexts;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.EntityTests
{
    [TestClass]
    public class BaseEntityTest : BaseTest
    {
        [TestInitialize]
        public void CleanDatabase()
        {
            using (AppUserStorageContext dbContext = new AppUserStorageContext())
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

        [TestMethod]
        public void AllEntityClassesShouldHaveDefaultEmptyConstructor()
        {
            var configurations =
                from t in typeof(BaseDbContext<>).Assembly.GetExportedTypes()
                let b = t.BaseType
                where b != null && b.IsGenericType && b.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)
                select t;
            configurations.Should().NotBeNullOrEmpty("doesn't help to verify entity classes if we can't find their configurations");

            var invalidTypes =
                from t in configurations
                from g in t.GetGenericArguments()
                let c = g.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null)
                where (c == null) || ((c.Attributes & MethodAttributes.Private) == MethodAttributes.Private)
                select g;

            invalidTypes.Should().BeNullOrEmpty("Entity Framework models need a public or protected parameterless constructor: " + string.Join(",", invalidTypes.Select(x => x.Name)));
        }
    }
}
