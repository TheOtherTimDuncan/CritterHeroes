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
    public class BreedTests : BaseEntityTest
    {
        [TestMethod]
        public async Task CanCreateReadAndDeleteBreed()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            Species species = new Species("species", "singular", "plural", null, null);

            Breed breed = new Breed(species, "breed");

            using (TestSqlStorageContext<Breed> storageContext = new TestSqlStorageContext<Breed>())
            {
                storageContext.FillWithTestData(breed, "ID", "SpeciesID");
                storageContext.Add(breed);
                await storageContext.SaveChangesAsync();
            }

            using (TestSqlStorageContext<Breed> storageContext = new TestSqlStorageContext<Breed>())
            {
                Breed result = await storageContext.Entities.FindByIDAsync(breed.ID);
                result.Should().NotBeNull();

                result.Species.ID.Should().Be(species.ID);
                result.BreedName.Should().Be(breed.BreedName);
                result.RescueGroupsID.Should().Be(breed.RescueGroupsID);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                Breed deleted = await storageContext.Entities.FindByIDAsync(breed.ID);
                deleted.Should().BeNull();
            }
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithInvalidName()
        {
            Action action = () => new Breed(null, null);
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("species");
        }
    }
}
