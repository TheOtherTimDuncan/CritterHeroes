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
    public class CritterTests : BaseEntityTest
    {
        [TestMethod]
        public async Task CanCreateReadAndDeleteCritter()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            Species species = new Species("species", "species", "species");

            Breed breed = new Breed(species, "breed");

            CritterStatus status = new CritterStatus("status", "description");

            Organization organization = new Organization()
            {
                FullName = "full",
                AzureName = "azure",
                EmailAddress = "email@emailcom"
            };

            Critter critter = new Critter("critter", status, breed, organization.ID);

            critter.WhenCreated.Should().BeCloseTo(DateTimeOffset.UtcNow);
            critter.WhenUpdated.Should().Be(critter.WhenCreated);

            using (SqlStorageContext<Organization> storageContext = new SqlStorageContext<Organization>())
            {
                storageContext.Add(organization);
                await storageContext.SaveChangesAsync();
            }

            using (SqlStorageContext<Critter> storageContext = new SqlStorageContext<Critter>())
            {
                EntityTestHelper.FillWithTestData(storageContext, critter, "StatusID", "WhenCreated", "WhenUpdated", "BreedID", "OrganizationID", "FosterID");
                storageContext.Add(critter);
                await storageContext.SaveChangesAsync();
            }

            using (SqlStorageContext<Critter> storageContext = new SqlStorageContext<Critter>())
            {
                Critter result = await storageContext.Entities.FindByIDAsync(critter.ID);
                result.Should().NotBeNull();

                result.RescueGroupsID.Should().Be(critter.RescueGroupsID);
                result.WhenCreated.Should().Be(critter.WhenCreated);
                result.WhenUpdated.Should().Be(critter.WhenUpdated);
                result.Name.Should().Be(critter.Name);
                result.Sex.Should().Be(critter.Sex);
                result.RescueGroupsLastUpdated.Should().Be(critter.RescueGroupsLastUpdated);
                result.RescueID.Should().Be(critter.RescueID);
                result.RescueGroupsCreated.Should().Be(critter.RescueGroupsCreated);
                result.RescueGroupsLastUpdated.Should().Be(critter.RescueGroupsLastUpdated);

                result.OrganizationID.Should().Be(organization.ID);
                result.Organization.Should().NotBeNull();
                result.Organization.ID.Should().Be(organization.ID);

                result.StatusID.Should().Be(status.ID);
                result.Status.Should().NotBeNull();
                result.Status.ID.Should().Be(status.ID);

                result.BreedID.Should().Be(breed.ID);
                result.Breed.Should().NotBeNull();
                result.Breed.ID.Should().Be(breed.ID);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                storageContext.Entities.FindByID(critter.ID).Should().BeNull();
            }
        }

        [TestMethod]
        public async Task CanCreateReadAndDeleteCritterPicture()
        {
            Picture picture = new Picture("picture", 1, 2, 3, "contentType");

            Organization organization = new Organization()
            {
                FullName = "full",
                AzureName = "azure",
                EmailAddress = "email@emailcom"
            };

            Critter critter = new Critter("name", new CritterStatus("status", "status"), new Breed(new Species("species", "singular", "plural"), "breed"), organization.ID)
            {
                Sex = "Male"
            };
            CritterPicture critterPicture = critter.AddPicture(picture);

            using (SqlStorageContext<Organization> storageContext = new SqlStorageContext<Organization>())
            {
                storageContext.Add(organization);
                await storageContext.SaveChangesAsync();
            }

            using (SqlStorageContext<Critter> storageContext = new SqlStorageContext<Critter>())
            {
                storageContext.Add(critter);
                await storageContext.SaveChangesAsync();
            }

            using (SqlStorageContext<Critter> storageContext = new SqlStorageContext<Critter>())
            {
                Critter result = await storageContext.Entities.FindByIDAsync(critter.ID);
                result.Should().NotBeNull();

                result.Pictures.Should().HaveCount(1);
                CritterPicture resultPicture = result.Pictures.Single();

                resultPicture.PictureID.Should().Be(picture.ID);
                resultPicture.Picture.Should().NotBeNull();
                resultPicture.Picture.ID.Should().Be(picture.ID);

                resultPicture.CritterID.Should().Be(critter.ID);
                resultPicture.Critter.Should().NotBeNull();
                resultPicture.Critter.ID.Should().Be(critter.ID);

                // Can we remove a picture?
                result.Pictures.Remove(resultPicture);
                await storageContext.SaveChangesAsync();
            }
        }

        [TestMethod]
        public async Task CanCreateReadAndDeleteCritterFoster()
        {
            Person person = new Person()
            {
                FirstName = "First",
                LastName = "Last"
            };

            Organization organization = new Organization()
            {
                FullName = "full",
                AzureName = "azure",
                EmailAddress = "email@emailcom"
            };

            Critter critter = new Critter("name", new CritterStatus("status", "status"), new Breed(new Species("species", "singular", "plural"), "breed"), organization.ID)
            {
                Sex = "Male"
            };
            critter.ChangeFoster(person);

            using (SqlStorageContext<Organization> storageContext = new SqlStorageContext<Organization>())
            {
                storageContext.Add(organization);
                await storageContext.SaveChangesAsync();
            }

            using (SqlStorageContext<Critter> storageContext = new SqlStorageContext<Critter>())
            {
                storageContext.Add(critter);
                await storageContext.SaveChangesAsync();
            }

            using (SqlStorageContext<Critter> storageContext = new SqlStorageContext<Critter>())
            {
                Critter result = await storageContext.Entities.FindByIDAsync(critter.ID);
                result.Should().NotBeNull();

                result.FosterID.Should().Be(person.ID);
                result.Foster.Should().NotBeNull();
                result.Foster.ID.Should().Be(person.ID);


                // Can we remove the foster?
                result.RemoveFoster();
                await storageContext.SaveChangesAsync();
            }
        }
    }
}
