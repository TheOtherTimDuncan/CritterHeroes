using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Azure;
using AR.Azure.Storage;
using AR.Domain.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.Azure.StorageEntityTests
{
    [TestClass]
    public class OrganizationStorageEntityTests
    {
        [TestMethod]
        public void SuccessfullyMapsEntityToStorage()
        {
            Organization organization = new Organization()
            {
                FullName = "FullName",
                ShortName = "ShortName",
                AzureTableName = "Azure",
                SupportedCritters = new string[] { "Critter1", "Critter2" }
            };

            StorageEntity<Organization> storageEntity = StorageEntityFactory.GetStorageEntity<Organization>();
            storageEntity.Should().NotBeNull();

            storageEntity.Entity = organization;
            storageEntity.TableEntity.Properties.Count.Should().Be(5);
            storageEntity.TableEntity["ID"].GuidValue.Should().Be(organization.ID);
            storageEntity.TableEntity["FullName"].StringValue.Should().Be(organization.FullName);
            storageEntity.TableEntity["ShortName"].StringValue.Should().Be(organization.ShortName);
            storageEntity.TableEntity["AzureTableName"].StringValue.Should().Be(organization.AzureTableName);
            storageEntity.TableEntity["SupportedCritters"].StringValue.Should().Be("Critter1|Critter2");
        }

        [TestMethod]
        public void SuccessfullyMapsStorageToEntity()
        {
            Organization organization = new Organization()
            {
                FullName = "FullName",
                ShortName = "ShortName",
                AzureTableName = "Azure",
                SupportedCritters = new string[] { "Critter1", "Critter2" }
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
            storageEntity2.Entity.AzureTableName.Should().Be(organization.AzureTableName);
            storageEntity2.Entity.SupportedCritters.Should().Equal(organization.SupportedCritters);
        }

        [TestMethod]
        public async Task TestCRUDSingle()
        {
            Organization organization = new Organization()
            {
                FullName = "FullName",
                ShortName = "ShortName",
                AzureTableName = "Azure",
                SupportedCritters = new string[] { "Critter1", "Critter2" }
            };

            AzureStorage storage = new AzureStorage("organization");
            await storage.SaveAsync<Organization>(organization);

            Organization result = await storage.GetAsync<Organization>(organization.ID.ToString());
            result.Should().NotBeNull();
            result.FullName.Should().Be(organization.FullName);
            result.ShortName.Should().Be(organization.ShortName);
            result.AzureTableName.Should().Be(organization.AzureTableName);
            result.SupportedCritters.Should().Equal(organization.SupportedCritters);

            await storage.DeleteAsync<Organization>(organization);
            Organization deleted = await storage.GetAsync<Organization>(organization.ID.ToString());
            deleted.Should().BeNull();
        }
    }
}
