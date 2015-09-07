using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
            using (BaseDbContext dbContext = new BaseDbContext())
            {
                DbMigrator migrator = new DbMigrator(new DatabaseMigrator.Migrations.Configuration());
                if (migrator.GetPendingMigrations().Any())
                {
                    Database.SetInitializer(new DropCreateDatabaseAlways<BaseDbContext>());
                    dbContext.Database.Initialize(force: true);
                }
                else
                {
                    IEnumerable<string> tableNames = new[]
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
                    };

                    foreach (string tableName in tableNames)
                    {
                        dbContext.Database.ExecuteSqlCommand($"DELETE FROM [{tableName}];");
                    }
                }
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
