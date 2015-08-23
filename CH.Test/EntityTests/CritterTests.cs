using System;
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
    public class CritterTests : BaseTest
    {
        [TestMethod]
        public async Task CanCreateReadAndDeleteCritter()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            Species species = new Species("critter-species", "species", "species", null, null);

            Breed breed = new Breed(1, species, "breed");

            CritterStatus status = new CritterStatus(2, "status", "description");

            Critter critter = new Critter(status, "critter", breed);

            critter.WhenCreated.Should().BeCloseTo(DateTimeOffset.UtcNow);
            critter.WhenUpdated.Should().Be(critter.WhenCreated);

            using (SqlStorageContext<Critter> storageContext = new SqlStorageContext<Critter>())
            {
                EntityTestHelper.FillWithTestData(storageContext, critter, "StatusID", "WhenCreated", "WhenUpdated", "BreedID");
                storageContext.Add(critter);
                await storageContext.SaveChangesAsync();
            }

            using (SqlStorageContext<Critter> storageContext = new SqlStorageContext<Critter>())
            {
                Critter result = await storageContext.FindByIDAsync(critter.ID);
                result.Should().NotBeNull();

                result.RescueGroupsID.Should().Be(critter.RescueGroupsID);
                result.WhenCreated.Should().Be(critter.WhenCreated);
                result.WhenUpdated.Should().Be(critter.WhenUpdated);
                result.Name.Should().Be(critter.Name);
                result.Sex.Should().Be(critter.Sex);

                result.StatusID.Should().Be(status.ID);
                result.Status.Should().NotBeNull();
                result.Status.ID.Should().Be(status.ID);

                result.BreedID.Should().Be(breed.ID);
                result.Breed.Should().NotBeNull();
                result.Breed.ID.Should().Be(breed.ID);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                storageContext.FindByID(critter.ID).Should().BeNull();
            }
        }
    }
}
