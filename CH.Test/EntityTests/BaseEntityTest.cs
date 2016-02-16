using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Data.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
    }
}
