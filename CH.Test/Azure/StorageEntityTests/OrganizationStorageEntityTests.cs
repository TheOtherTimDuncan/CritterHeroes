using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Azure;
using CH.Azure.Storage;
using CH.Domain.Models;
using CH.Domain.Models.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.Azure.StorageEntityTests
{
    [TestClass]
    public class OrganizationStorageEntityTests : BaseStorageEntityTest
    {
        [TestMethod]
        public void SuccessfullyMapsEntityToStorage()
        {
            Organization organization = new Organization()
            {
                FullName = "FullName",
                ShortName = "ShortName",
                AzureName = "Azure",
                LogoFilename = "Logo",
                SupportedCritters = new Species[] 
                { 
                    new Species("1","singular-1","plural-2", null, null),
                    new Species("2","singular-2","plural-2", null, null) 
                }
            };

            StorageEntity<Organization> storageEntity = StorageEntityFactory.GetStorageEntity<Organization>();
            storageEntity.Should().NotBeNull();

            storageEntity.Entity = organization;
            storageEntity.TableEntity.Properties.Count.Should().Be(6);
            storageEntity.TableEntity["ID"].GuidValue.Should().Be(organization.ID);
            storageEntity.TableEntity["FullName"].StringValue.Should().Be(organization.FullName);
            storageEntity.TableEntity["ShortName"].StringValue.Should().Be(organization.ShortName);
            storageEntity.TableEntity["AzureName"].StringValue.Should().Be(organization.AzureName);
            storageEntity.TableEntity["LogoFilename"].StringValue.Should().Be(organization.LogoFilename);
            storageEntity.TableEntity["SupportedCritters"].StringValue.Should().Be("[{\"Name\":\"1\",\"Singular\":\"singular-1\",\"Plural\":\"plural-2\",\"YoungSingular\":null,\"YoungPlural\":null},{\"Name\":\"2\",\"Singular\":\"singular-2\",\"Plural\":\"plural-2\",\"YoungSingular\":null,\"YoungPlural\":null}]");
        }

        [TestMethod]
        public void SuccessfullyMapsEntityToStorageWithNullSupportedCritters()
        {
            Organization organization = new Organization()
            {
                FullName = "FullName",
                ShortName = "ShortName",
                AzureName = "Azure",
                LogoFilename = "Logo",
                SupportedCritters = null
            };

            StorageEntity<Organization> storageEntity = StorageEntityFactory.GetStorageEntity<Organization>();
            storageEntity.Should().NotBeNull();

            storageEntity.Entity = organization;
            storageEntity.TableEntity["SupportedCritters"].StringValue.Should().Be("null");
        }

        [TestMethod]
        public void SuccessfullyMapsStorageToEntity()
        {
            Organization organization = new Organization()
            {
                FullName = "FullName",
                ShortName = "ShortName",
                AzureName = "Azure",
                LogoFilename = "Logo",
                SupportedCritters = new Species[] 
                { 
                    new Species("1","singular-1","plural-2", null, null),
                    new Species("2","singular-2","plural-2", null, null) 
                }
            };

            StorageEntity<Organization> storageEntity1 = StorageEntityFactory.GetStorageEntity<Organization>();
            storageEntity1.Should().NotBeNull();
            storageEntity1.Entity = organization;

            StorageEntity<Organization> storageEntity2 = StorageEntityFactory.GetStorageEntity<Organization>();
            storageEntity2.Should().NotBeNull();
            storageEntity2.TableEntity = storageEntity1.TableEntity;

            storageEntity2.Entity.ID.Should().Be(organization.ID);
            storageEntity2.Entity.FullName.Should().Be(organization.FullName);
            storageEntity2.Entity.ShortName.Should().Be(organization.ShortName);
            storageEntity2.Entity.AzureName.Should().Be(organization.AzureName);
            storageEntity2.Entity.LogoFilename.Should().Be(organization.LogoFilename);
            storageEntity2.Entity.SupportedCritters.Should().Equal(organization.SupportedCritters);
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
                SupportedCritters = null
            };

            StorageEntity<Organization> storageEntity1 = StorageEntityFactory.GetStorageEntity<Organization>();
            storageEntity1.Should().NotBeNull();
            storageEntity1.Entity = organization;

            StorageEntity<Organization> storageEntity2 = StorageEntityFactory.GetStorageEntity<Organization>();
            storageEntity2.Should().NotBeNull();
            storageEntity2.TableEntity = storageEntity1.TableEntity;

            storageEntity2.Entity.SupportedCritters.Should().BeNull();
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
                SupportedCritters = new Species[] 
                { 
                    new Species("1","singular-1","plural-2", null, null),
                    new Species("2","singular-2","plural-2", null, null) 
                }
            };

            AzureStorage storage = GetAzureStorage("organization");
            await storage.SaveAsync<Organization>(organization);

            Organization result = await storage.GetAsync<Organization>(organization.ID.ToString());
            result.Should().NotBeNull();
            result.FullName.Should().Be(organization.FullName);
            result.ShortName.Should().Be(organization.ShortName);
            result.AzureName.Should().Be(organization.AzureName);
            result.LogoFilename.Should().Be(organization.LogoFilename);
            result.SupportedCritters.Should().Equal(organization.SupportedCritters);

            await storage.DeleteAsync<Organization>(organization);
            Organization deleted = await storage.GetAsync<Organization>(organization.ID.ToString());
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
                SupportedCritters = new Species[] 
                { 
                    new Species("Cat","Cat","Cats", "Kitten", "Kittens"),
                    new Species("Dog","Dog","Dogs", "Puppy", "Puppies") 
                }
            };

            AzureStorage storage = GetAzureStorage("organization");
            await storage.SaveAsync<Organization>(organization);
        }
    }
}
