﻿using System;
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
    public class LocationTests : BaseEntityTest
    {
        [TestMethod]
        public async Task CanCreateReadAndDeleteLocation()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            Location location = new Location("test");

            using (TestSqlStorageContext<Location> storageContext = new TestSqlStorageContext<Location>())
            {
                storageContext.FillWithTestData(location);
                storageContext.Add(location);
                await storageContext.SaveChangesAsync();
            }

            using (TestSqlStorageContext<Location> storageContext = new TestSqlStorageContext<Location>())
            {
                Location result = await storageContext.Entities.FindByIDAsync(location.ID);
                result.Should().NotBeNull();

                result.Name.Should().Be(location.Name);
                result.Address.Should().Be(location.Address);
                result.City.Should().Be(location.City);
                result.State.Should().Be(location.State);
                result.Zip.Should().Be(location.Zip);
                result.Phone.Should().Be(location.Phone);
                result.Website.Should().Be(location.Website);
                result.RescueGroupsID.Should().Be(location.RescueGroupsID);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                storageContext.Entities.MatchingID(location.ID).SingleOrDefault().Should().BeNull();
            }
        }
    }
}
