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

            using (SqlStorageContext<Organization> storageContext = new SqlStorageContext<Organization>())
            {
                EntityTestHelper.FillWithTestData(storageContext, organization, "ID");
                storageContext.Add(organization);
                await storageContext.SaveChangesAsync();
            }

            using (SqlStorageContext<Organization> storageContext = new SqlStorageContext<Organization>())
            {
                Organization result = await storageContext.FindByIDAsync(organization.ID);
                result.Should().NotBeNull();

                result.FullName.Should().Be(organization.FullName);
                result.ShortName.Should().Be(organization.ShortName);
                result.AzureName.Should().Be(organization.AzureName);
                result.LogoFilename.Should().Be(organization.LogoFilename);
                result.EmailAddress.Should().Be(organization.EmailAddress);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                Organization deleted = await storageContext.FindByIDAsync(organization.ID);
                deleted.Should().BeNull();
            }
        }
    }
}
