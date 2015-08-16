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
    public class BreedTests : BaseEntityTest
    {
        [TestMethod]
        public async Task CanCreateReadAndDeleteBreed()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            Breed breed = new Breed(1, "species", "breed");

            using (SqlStorageContext<Breed> storageContext = new SqlStorageContext<Breed>())
            {
                EntityTestHelper.FillWithTestData(storageContext, breed, "ID");
                storageContext.Add(breed);
                await storageContext.SaveChangesAsync();
            }

            using (SqlStorageContext<Breed> storageContext = new SqlStorageContext<Breed>())
            {
                Breed result = await storageContext.FindByIDAsync(breed.ID);
                result.Should().NotBeNull();

                result.Species.Should().Be(breed.Species);
                result.BreedName.Should().Be(breed.BreedName);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                Breed deleted = await storageContext.FindByIDAsync(breed.ID);
                deleted.Should().BeNull();
            }
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithInvalidName()
        {
            Action action = () => new Breed(1, null, null);
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("species");
        }
    }
}
