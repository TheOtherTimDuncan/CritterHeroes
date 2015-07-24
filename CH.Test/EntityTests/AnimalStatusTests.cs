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
    public class AnimalStatusTests : BaseEntityTest
    {
        [TestMethod]
        public async Task CanCreateReadAndDeleteAnimalStatus()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            AnimalStatus animalStatus = new AnimalStatus(1, "name", "description");

            using (AnimalStatusStorageContext storageContext = new AnimalStatusStorageContext())
            {
                EntityTestHelper.FillWithTestData(storageContext, animalStatus, "ID");
                storageContext.Add(animalStatus);
                await storageContext.SaveChangesAsync();
            }

            using (AnimalStatusStorageContext storageContext = new AnimalStatusStorageContext())
            {
                AnimalStatus result = await storageContext.GetAsync(x => x.ID == animalStatus.ID);
                result.Should().NotBeNull();

                result.Name.Should().Be(animalStatus.Name);
                result.Description.Should().Be(animalStatus.Description);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                AnimalStatus deleted = await storageContext.GetAsync(x => x.ID == animalStatus.ID);
                deleted.Should().BeNull();
            }
        }

    }
}
