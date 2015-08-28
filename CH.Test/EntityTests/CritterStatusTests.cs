﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Contexts;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.EntityFramework;

namespace CH.Test.EntityTests
{
    [TestClass]
    public class CritterStatusTests : BaseEntityTest
    {
        [TestMethod]
        public async Task CanCreateReadAndDeleteAnimalStatus()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            CritterStatus critterStatus = new CritterStatus("name", "description");

            using (SqlStorageContext<CritterStatus> storageContext = new SqlStorageContext<CritterStatus>())
            {
                EntityTestHelper.FillWithTestData(storageContext, critterStatus, "ID");
                storageContext.Add(critterStatus);
                await storageContext.SaveChangesAsync();
            }

            using (SqlStorageContext<CritterStatus> storageContext = new SqlStorageContext<CritterStatus>())
            {
                CritterStatus result = await storageContext.FindByIDAsync(critterStatus.ID);
                result.Should().NotBeNull();

                result.Name.Should().Be(critterStatus.Name);
                result.Description.Should().Be(critterStatus.Description);
                result.RescueGroupsID.Should().Be(critterStatus.RescueGroupsID);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                CritterStatus deleted = await storageContext.FindByIDAsync(critterStatus.ID);
                deleted.Should().BeNull();
            }
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithInvalidName()
        {
            Action action = () => new CritterStatus(1, null, null);
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("name");
        }
    }
}