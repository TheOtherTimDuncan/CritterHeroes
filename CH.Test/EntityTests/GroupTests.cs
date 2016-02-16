using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Data.Contexts;
using CritterHeroes.Web.Data.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TOTD.EntityFramework;

namespace CH.Test.EntityTests
{
    [TestClass]
    public class GroupTests : BaseEntityTest
    {
        [TestMethod]
        public async Task CanCreateReadAndDeleteGroup()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            Group group = new Group("test");

            using (TestSqlStorageContext<Group> storageContext = new TestSqlStorageContext<Group>())
            {
                storageContext.FillWithTestData(group, "ID");
                storageContext.Add(group);
                await storageContext.SaveChangesAsync();
            }

            using (TestSqlStorageContext<Group> storageContext = new TestSqlStorageContext<Group>())
            {
                Group result = await storageContext.Entities.SingleOrDefaultAsync(x => x.ID == group.ID);
                result.Should().NotBeNull();

                result.Name.Should().Be(group.Name);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                storageContext.Entities.SingleOrDefault(x => x.ID == group.ID).Should().BeNull();
            }
        }
    }
}
