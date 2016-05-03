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
    public class OrganizationTests : BaseEntityTest
    {
        [TestMethod]
        public async Task CanCreateReadAndDeleteOrganization()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            Organization organization = new Organization();

            using (TestSqlStorageContext<Organization> storageContext = new TestSqlStorageContext<Organization>())
            {
                storageContext.FillWithTestData(organization, "ID");
                storageContext.Add(organization);
                await storageContext.SaveChangesAsync();
            }

            using (TestSqlStorageContext<Organization> storageContext = new TestSqlStorageContext<Organization>())
            {
                Organization result = await storageContext.Entities.FindByIDAsync(organization.ID);
                result.Should().NotBeNull();

                result.FullName.Should().Be(organization.FullName);
                result.ShortName.Should().Be(organization.ShortName);
                result.RescueGroupsID.Should().Be(organization.RescueGroupsID);
                result.AzureName.Should().Be(organization.AzureName);
                result.LogoFilename.Should().Be(organization.LogoFilename);
                result.EmailAddress.Should().Be(organization.EmailAddress);
                result.TimeZoneID.Should().Be(organization.TimeZoneID);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                Organization deleted = await storageContext.Entities.FindByIDAsync(organization.ID);
                deleted.Should().BeNull();
            }
        }

        [TestMethod]
        public async Task CanCreateAndReadOrganizationSupportedCritter()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            Species species1 = new Species("1", "singular1", "plural1", null, null);
            Species species2 = new Species("2", "singular2", "plural2", null, null);

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

            using (TestSqlStorageContext<Organization> storageContext = new TestSqlStorageContext<Organization>())
            {
                Organization result = await storageContext.Entities.FindByIDAsync(organization.ID);
                result.Should().NotBeNull();

                result.SupportedCritters.Should().HaveCount(2);

                result.SupportedCritters.First().Species.ID.Should().Be(species1.ID);
                result.SupportedCritters.Last().Species.ID.Should().Be(species2.ID);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                Organization deleted = await storageContext.Entities.FindByIDAsync(organization.ID);
                deleted.Should().BeNull();
            }
        }
    }
}
