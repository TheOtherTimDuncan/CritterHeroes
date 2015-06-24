using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Storage;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.Azure.StorageTests
{
    [TestClass]
    public class OrganizationStorageEntityTests : BaseAzureTest
    {
        [TestMethod]
        public void SuccessfullyMapsEntityToAndFromStorage()
        {
            Organization organization = new Organization()
            {
                FullName = "FullName",
                ShortName = "ShortName",
                AzureName = "Azure",
                LogoFilename = "Logo",
                EmailAddress = "email@email.com",
                SupportedCritters = new Species[] 
                { 
                    new Species("1","singular-1","plural-2", null, null),
                    new Species("2","singular-2","plural-2", null, null) 
                }
            };

            OrganizationAzureStorage source = new OrganizationAzureStorage(new AzureConfiguration());
            OrganizationAzureStorage target = new OrganizationAzureStorage(new AzureConfiguration());
            Organization result = target.FromStorage(source.ToStorage(organization));

            result.ID.Should().Be(organization.ID);
            result.FullName.Should().Be(organization.FullName);
            result.ShortName.Should().Be(organization.ShortName);
            result.AzureName.Should().Be(organization.AzureName);
            result.LogoFilename.Should().Be(organization.LogoFilename);
            result.EmailAddress.Should().Be(organization.EmailAddress);
            result.SupportedCritters.Should().Equal(organization.SupportedCritters);
        }

        [TestMethod]
        public void SuccessfullyMapsStorageWithNullSupportedCrittersToEntity()
        {
            Organization organization = new Organization()
            {
                FullName = "FullName",
                ShortName = "ShortName",
                AzureName = "Azure",
                LogoFilename = "Logo",
                EmailAddress = "email@email.com",
                SupportedCritters = null
            };

            OrganizationAzureStorage source = new OrganizationAzureStorage(new AzureConfiguration());
            OrganizationAzureStorage target = new OrganizationAzureStorage(new AzureConfiguration());
            Organization result = target.FromStorage(source.ToStorage(organization));

            result.SupportedCritters.Should().BeNull();
        }

        [TestMethod]
        public async Task TestCRUDSingle()
        {
            Organization organization = new Organization()
            {
                FullName = "FullName",
                ShortName = "ShortName",
                AzureName = "Azure",
                LogoFilename = "Logo",
                EmailAddress = "email@email.com",
                SupportedCritters = new Species[] 
                { 
                    new Species("1","singular-1","plural-2", null, null),
                    new Species("2","singular-2","plural-2", null, null) 
                }
            };

            OrganizationAzureStorage storage = new OrganizationAzureStorage(new AzureConfiguration());
            await storage.SaveAsync(organization);

            Organization result = await storage.GetAsync(organization.ID.ToString());
            result.Should().NotBeNull();
            result.FullName.Should().Be(organization.FullName);
            result.ShortName.Should().Be(organization.ShortName);
            result.AzureName.Should().Be(organization.AzureName);
            result.LogoFilename.Should().Be(organization.LogoFilename);
            result.EmailAddress.Should().Be(organization.EmailAddress);
            result.SupportedCritters.Should().Equal(organization.SupportedCritters);

            await storage.DeleteAsync(organization);
            Organization deleted = await storage.GetAsync(organization.ID.ToString());
            deleted.Should().BeNull();
        }

        //[TestMethod]
        public async Task Seed()
        {
            Organization organization = new Organization(Guid.Parse("71a22c0b-23fb-4fc0-96a8-792474c80953"))
            {
                FullName = "Friends For Life Animal Haven",
                ShortName = "FFLAH",
                AzureName = "fflah",
                LogoFilename = "logo.svg",
                EmailAddress = "email@email.com",
                SupportedCritters = new Species[] 
                { 
                    new Species("Cat","Cat","Cats", "Kitten", "Kittens"),
                    new Species("Dog","Dog","Dogs", "Puppy", "Puppies") 
                }
            };

            OrganizationAzureStorage storage = new OrganizationAzureStorage(new AzureConfiguration());
            await storage.SaveAsync(organization);
        }
    }
}
