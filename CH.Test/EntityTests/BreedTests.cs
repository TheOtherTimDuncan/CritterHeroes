using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Contexts;
using CritterHeroes.Web.Data.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.EntityFramework;

namespace CH.Test.EntityTests
{
    [TestClass]
    public class BreedTests : BaseEntityTest
    {
        [TestMethod]
        public async Task CanCreateReadAndDeleteBreed()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            Breed breed = new Breed(1, "species", "breed");

            using (BreedStorageContext storageContext = new BreedStorageContext())
            {
                EntityTestHelper.FillWithTestData(storageContext, breed, "ID");
                storageContext.Add(breed);
                await storageContext.SaveChangesAsync();
            }

            using (BreedStorageContext storageContext = new BreedStorageContext())
            {
                Breed result = await storageContext.GetAsync(x => x.ID == breed.ID);
                result.Should().NotBeNull();

                result.Species.Should().Be(breed.Species);
                result.BreedName.Should().Be(breed.BreedName);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                Breed deleted = await storageContext.GetAsync(x => x.ID == breed.ID);
                deleted.Should().BeNull();
            }
        }
    }
}
