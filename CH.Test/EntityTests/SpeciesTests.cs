using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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

            using (TestSqlStorageContext<Species> storageContext = new TestSqlStorageContext<Species>())
            {
                storageContext.FillWithTestData(species, "ID");
                storageContext.Add(species);
                await storageContext.SaveChangesAsync();
            }

            using (TestSqlStorageContext<Species> storageContext = new TestSqlStorageContext<Species>())
            {
                Species result = await storageContext.Entities.FindByNameAsync(species.Name);
                result.Should().NotBeNull();

                result.Singular.Should().Be(species.Singular);
                result.Plural.Should().Be(species.Plural);
                result.YoungPlural.Should().Be(species.YoungPlural);
                result.YoungSingular.Should().Be(species.YoungSingular);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                Species deleted = await storageContext.Entities.FindByNameAsync(species.Name);
                deleted.Should().BeNull();
            }
        }

        [TestMethod]
        public async Task CanReadOrganizationSupportedCritters()
        {
            Species species1 = new Species("org1", "singular1", "plural1", null, null);
            Species species2 = new Species("org2", "singular2", "plural2", null, null);

            Organization organization = new Organization()
            {
                FullName = "FullName",
                AzureName = "Azure",
                EmailAddress = "email@email.com"
            };

            organization.AddSupportedCritter(species1);
            organization.AddSupportedCritter(species2);

            using (TestSqlStorageContext<Organization> storageContext = new TestSqlStorageContext<Organization>())
            {
                storageContext.Add(organization);
                await storageContext.SaveChangesAsync();
            }

            using (TestSqlStorageContext<Species> storageContext = new TestSqlStorageContext<Species>())
            {
                Species result1 = await storageContext.Entities.FindByNameAsync(species1.Name);
                result1.OrganizationSupportedCritters.Should().HaveCount(1);
                result1.OrganizationSupportedCritters.Single().OrganizationID.Should().Be(organization.ID);

                Species result2 = await storageContext.Entities.FindByNameAsync(species2.Name);
                result2.OrganizationSupportedCritters.Should().HaveCount(1);
                result2.OrganizationSupportedCritters.Single().OrganizationID.Should().Be(organization.ID);

                storageContext.Delete(result1);
                Action action = () => storageContext.SaveChanges();
                action.ShouldThrow<InvalidOperationException>("foreign key relationship to OrganizationSupportedCritters should prevent Species from being deleted");
            }
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithNullName()
        {
            Action action = () => new Species(null, "singular", "plural", "youngSingular", "youngPlural");
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithEmptyName()
        {
            Action action = () => new Species("", "singular", "plural", "youngSingular", "youngPlural");
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithNullSingular()
        {
            Action action = () => new Species("name", null, "plural", "youngSingular", "youngPlural");
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("singular");
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithEmptySingular()
        {
            Action action = () => new Species("name", "", "plural", "youngSingular", "youngPlural");
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("singular");
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithNullPlural()
        {
            Action action = () => new Species("name", "singular", null, "youngSingular", "youngPlural");
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("plural");
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithEmptyPlural()
        {
            Action action = () => new Species("name", "singular", "", "youngSingular", "youngPlural");
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("plural");
        }
    }
}
