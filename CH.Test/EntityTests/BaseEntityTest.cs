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
        [AssemblyInitialize]
        public static void ResetDatabase(TestContext testContext)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<BaseDbContext>());
            using (BaseDbContext dbContext = new BaseDbContext())
            {
                dbContext.Database.Initialize(force: true);
            }
        }

        [AssemblyCleanup]
        public static void DeleteDatabase()
        {
            using (BaseDbContext dbContext = new BaseDbContext())
            {
                dbContext.Database.Delete();
            }
        }

        [TestMethod]
        public void AllEntityClassesShouldHaveDefaultEmptyConstructor()
        {
            var invalidTypes =
                from t in typeof(BaseDbContext).Assembly.GetExportedTypes()
                let b = t.BaseType
                where b != null && b.IsGenericType && b.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)
                from g in b.GetGenericArguments()
                let c = g.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null)
                where (c == null) || ((c.Attributes & MethodAttributes.Private) == MethodAttributes.Private)
                select g;

            invalidTypes.Should().BeNullOrEmpty("Entity Framework models need a public or protected parameterless constructor: " + string.Join(",", invalidTypes.Select(x => x.Name)));
        }
    }
}
