using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace CH.Test.RescueGroups
{
    [TestClass]
    public class CritterSourceTests
    {
        [TestMethod]
        public void ObjectTypeIsCorrect()
        {
            new CritterSourceStorage(new RescueGroupsConfiguration(), null, null).ObjectType.Should().Be("animals");
        }

        [TestMethod]
        public void ConvertsCritterJsonResultToModel()
        {
            CritterSource critterSource1 = new CritterSource()
            {
                ID = 1,
                Name = "Name1",
                StatusID = "2",
                Status = "Status1",
                Species = "Species1",
                PrimaryBreedID = "3",
                PrimaryBreed = "Breed1",
                Sex = "Sex1",
                RescueID = "RescueID1",
                LastUpdated = new DateTime(2001, 1, 1, 1, 1, 1),
                Created = new DateTime(2002, 2, 2, 2, 2, 2),
                FosterContactID = "4",
                FosterFirstName = "FosterFirstName1",
                FosterLastName = "FosterLastName1",
                FosterEmail = "FosterEmail1",
                LocationID = "LocationID1",
                LocationName = "LocationName1"
            };

            CritterSource critterSource2 = new CritterSource()
            {
                ID = 2,
                Name = "Name2",
                StatusID = "5",
                Status = "Status2",
                Species = "Species2",
                PrimaryBreedID = "6",
                PrimaryBreed = "Breed2",
                Sex = "Sex2",
                RescueID = "RescueID2",
                LastUpdated = new DateTime(2003, 3, 3, 3, 3, 3),
                Created = new DateTime(2004, 4, 4, 4, 4, 4),
                FosterContactID = "7",
                FosterFirstName = "FosterFirstName2",
                FosterLastName = "FosterLastName2",
                FosterEmail = "FosterEmail2",
                LocationID = "LocationID2",
                LocationName = "LocationName2"
            };

            JProperty property1 = new JProperty(critterSource1.ID.ToString(), new JObject(
                new JProperty("animalID", critterSource1.ID),
                new JProperty("animalName", critterSource1.Name),
                new JProperty("animalSex", critterSource1.Sex),
                new JProperty("animalStatusID", critterSource1.StatusID),
                new JProperty("animalStatus", critterSource1.Status),
                new JProperty("animalPrimaryBreedID", critterSource1.PrimaryBreedID),
                new JProperty("animalPrimaryBreed", critterSource1.PrimaryBreed),
                new JProperty("animalSpecies", critterSource1.Species),
                new JProperty("animalFosterID", critterSource1.FosterContactID),
                new JProperty("fosterFirstname", critterSource1.FosterFirstName),
                new JProperty("fosterLastname", critterSource1.FosterLastName),
                new JProperty("fosterEmail", critterSource1.FosterEmail),
                new JProperty("animalRescueID", critterSource1.RescueID),
                new JProperty("animalUpdatedDate", critterSource1.LastUpdated),
                new JProperty("animalCreatedDate", critterSource1.Created),
                new JProperty("animalLocationID", critterSource1.LocationID),
                new JProperty("locationName", critterSource1.LocationName)
            ));

            JProperty property2 = new JProperty(critterSource2.ID.ToString(), new JObject(
                new JProperty("animalID", critterSource2.ID),
                new JProperty("animalName", critterSource2.Name),
                new JProperty("animalSex", critterSource2.Sex),
                new JProperty("animalStatusID", critterSource2.StatusID),
                new JProperty("animalStatus", critterSource2.Status),
                new JProperty("animalPrimaryBreedID", critterSource2.PrimaryBreedID),
                new JProperty("animalPrimaryBreed", critterSource2.PrimaryBreed),
                new JProperty("animalSpecies", critterSource2.Species),
                new JProperty("animalFosterID", critterSource2.FosterContactID),
                new JProperty("fosterFirstname", critterSource2.FosterFirstName),
                new JProperty("fosterLastname", critterSource2.FosterLastName),
                new JProperty("fosterEmail", critterSource2.FosterEmail),
                new JProperty("animalRescueID", critterSource2.RescueID),
                new JProperty("animalUpdatedDate", critterSource2.LastUpdated),
                new JProperty("animalCreatedDate", critterSource2.Created),
                new JProperty("animalLocationID", critterSource2.LocationID),
                new JProperty("locationName", critterSource2.LocationName)
            ));

            JObject data = new JObject(property1, property2);

            IEnumerable<CritterSource> critters = new CritterSourceStorage(new RescueGroupsConfiguration(), null, null).FromStorage(data.Properties());
            critters.Should().HaveCount(2);

            CritterSource result1 = critters.SingleOrDefault(x => x.ID == critterSource1.ID);
            result1.Should().NotBeNull();

            result1.Name.Should().Be(critterSource1.Name);
            result1.Sex.Should().Be(critterSource1.Sex);
            result1.StatusID.Should().Be(critterSource1.StatusID);
            result1.Status.Should().Be(critterSource1.Status);
            result1.PrimaryBreedID.Should().Be(critterSource1.PrimaryBreedID);
            result1.PrimaryBreed.Should().Be(critterSource1.PrimaryBreed);
            result1.Species.Should().Be(critterSource1.Species);
            result1.FosterContactID.Should().Be(critterSource1.FosterContactID);
            result1.FosterFirstName.Should().Be(critterSource1.FosterFirstName);
            result1.FosterLastName.Should().Be(critterSource1.FosterLastName);
            result1.FosterEmail.Should().Be(critterSource1.FosterEmail);
            result1.RescueID.Should().Be(critterSource1.RescueID);
            result1.LastUpdated.Should().Be(critterSource1.LastUpdated);
            result1.Created.Should().Be(critterSource1.Created);
            result1.LocationID.Should().Be(critterSource1.LocationID);
            result1.LocationName.Should().Be(critterSource1.LocationName);

            CritterSource result2 = critters.SingleOrDefault(x => x.ID == critterSource2.ID);
            result2.Should().NotBeNull();

            result2.Name.Should().Be(critterSource2.Name);
            result2.Sex.Should().Be(critterSource2.Sex);
            result2.StatusID.Should().Be(critterSource2.StatusID);
            result2.Status.Should().Be(critterSource2.Status);
            result2.PrimaryBreedID.Should().Be(critterSource2.PrimaryBreedID);
            result2.PrimaryBreed.Should().Be(critterSource2.PrimaryBreed);
            result2.Species.Should().Be(critterSource2.Species);
            result2.FosterContactID.Should().Be(critterSource2.FosterContactID);
            result2.FosterFirstName.Should().Be(critterSource2.FosterFirstName);
            result2.FosterLastName.Should().Be(critterSource2.FosterLastName);
            result2.FosterEmail.Should().Be(critterSource2.FosterEmail);
            result2.RescueID.Should().Be(critterSource2.RescueID);
            result2.LastUpdated.Should().Be(critterSource2.LastUpdated);
            result2.Created.Should().Be(critterSource2.Created);
            result2.LocationID.Should().Be(critterSource2.LocationID);
            result2.LocationName.Should().Be(critterSource2.LocationName);
        }

        [TestMethod]
        public void ConvertsCritterPictureJsonResultToModel()
        {
            CritterSource critterSource1 = new CritterSource()
            {
                ID = 1,
                Name = "Name1"
            };

            CritterPictureSource picture1 = new CritterPictureSource()
            {
                ID = "3",
                DisplayOrder = 44,
                LastUpdated = "01/01/2001 1:01 AM",
                FileSize = 123456,
                Width = 800,
                Height = 900,
                Filename = "Filename1",
                Url = "Url1",

                LargePicture = new PictureChildSource()
                {
                    FileSize = 8888,
                    Width = 111,
                    Height = 222,
                    Url = "LargeUrl"
                },

                SmallPicture = new PictureChildSource()
                {
                    FileSize = 9999,
                    Width = 333,
                    Height = 444,
                    Url = "SmallUrl"
                }
            };

            JArray jsonPicture1 = new JArray(
                new JObject(
                    new JProperty("mediaID", picture1.ID),
                    new JProperty("mediaOrder", picture1.DisplayOrder),
                    new JProperty("lastUpdated", picture1.LastUpdated),
                    new JProperty("fileSize", picture1.FileSize),
                    new JProperty("resolutionX", picture1.Width),
                    new JProperty("resolutionY", picture1.Height),
                    new JProperty("fileNameFullsize", picture1.Filename),
                    new JProperty("urlSecureFullsize", picture1.Url),
                    new JProperty("large", JToken.FromObject(picture1.LargePicture)),
                    new JProperty("small", JToken.FromObject(picture1.SmallPicture))
                )
            );

            JProperty property1 = new JProperty(critterSource1.ID.ToString(), new JObject(
                new JProperty("animalID", critterSource1.ID),
                new JProperty("animalName", critterSource1.Name),
                new JProperty("animalPictures", jsonPicture1)
            ));

            JObject data = new JObject(property1);

            IEnumerable<CritterSource> critters = new CritterSourceStorage(new RescueGroupsConfiguration(), null, null).FromStorage(data.Properties());

            CritterSource result1 = critters.SingleOrDefault(x => x.ID == critterSource1.ID);
            result1.Should().NotBeNull();

            result1.PictureSources.Should().HaveCount(1);
            CritterPictureSource sourcePicture1 = result1.PictureSources.Single();
            sourcePicture1.ID.Should().Be(picture1.ID);
            sourcePicture1.DisplayOrder.Should().Be(picture1.DisplayOrder);
            sourcePicture1.LastUpdated.Should().Be(picture1.LastUpdated);
            sourcePicture1.FileSize.Should().Be(picture1.FileSize);
            sourcePicture1.Width.Should().Be(picture1.Width);
            sourcePicture1.Height.Should().Be(picture1.Height);
            sourcePicture1.Filename.Should().Be(picture1.Filename);
            sourcePicture1.Url.Should().Be(picture1.Url);

            sourcePicture1.LargePicture.Should().NotBeNull();
            sourcePicture1.LargePicture.FileSize.Should().Be(picture1.LargePicture.FileSize);
            sourcePicture1.LargePicture.Width.Should().Be(picture1.LargePicture.Width);
            sourcePicture1.LargePicture.Height.Should().Be(picture1.LargePicture.Height);
            sourcePicture1.LargePicture.Url.Should().Be(picture1.LargePicture.Url);

            sourcePicture1.SmallPicture.Should().NotBeNull();
            sourcePicture1.SmallPicture.FileSize.Should().Be(picture1.SmallPicture.FileSize);
            sourcePicture1.SmallPicture.Width.Should().Be(picture1.SmallPicture.Width);
            sourcePicture1.SmallPicture.Height.Should().Be(picture1.SmallPicture.Height);
            sourcePicture1.SmallPicture.Url.Should().Be(picture1.SmallPicture.Url);
        }
    }
}
