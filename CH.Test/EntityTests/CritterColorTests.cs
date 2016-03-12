using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.EntityTests
{
    [TestClass]
    public class CritterColorTests : BaseEntityTest
    {
        [TestMethod]
        public async Task CanCreateReadAndDeleteCritterColor()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            CritterColor color = new CritterColor("color");

            using (TestSqlStorageContext<CritterColor> storageContext = new TestSqlStorageContext<CritterColor>())
            {
                storageContext.FillWithTestData(color);
                storageContext.Add(color);
                await storageContext.SaveChangesAsync();
            }

            using (TestSqlStorageContext<CritterColor> storageContext = new TestSqlStorageContext<CritterColor>())
            {
                CritterColor result = await storageContext.Entities.FindByIDAsync(color.ID);
                result.Should().NotBeNull();

                result.Description.Should().Be(color.Description);
                result.RescueGroupsID.Should().Be(color.RescueGroupsID);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                CritterColor deleted = await storageContext.Entities.FindByIDAsync(color.ID);
                deleted.Should().BeNull();
            }
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithInvalidName()
        {
            Action action = () => new CritterColor(null);
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("description");
        }
    }
}
