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
    public class SpeciesTests : BaseEntityTest
    {
        [TestMethod]
        public async Task CanCreateReadAndDeleteSpecies()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            Species species = new Species("name", "singular", "plural", "youngsingular", "youngplural");

            using (SqlStorageContext<Species> storageContext = new SqlStorageContext<Species>())
            {
                EntityTestHelper.FillWithTestData(storageContext, species);
                storageContext.Add(species);
                await storageContext.SaveChangesAsync();
            }

            using (SqlStorageContext<Species> storageContext = new SqlStorageContext<Species>())
            {
                Species result = await storageContext.FindByNameAsync(species.Name);
                result.Should().NotBeNull();

                result.Singular.Should().Be(species.Singular);
                result.Plural.Should().Be(species.Plural);
                result.YoungPlural.Should().Be(species.YoungPlural);
                result.YoungSingular.Should().Be(species.YoungSingular);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                Species deleted = await storageContext.FindByNameAsync(species.Name);
                deleted.Should().BeNull();
            }
        }
    }
}
