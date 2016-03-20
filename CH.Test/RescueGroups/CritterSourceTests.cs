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
                BirthDate = new DateTime(2001, 1, 1),
                IsBirthDateExact = true,
                ColorID = "12",
                Color = "Color1",
                IsCourtesy = false,
                Created = new DateTime(2001, 2, 2, 2, 2, 2),
                Description = "Description1",
                FosterContactID = "13",
                FosterFirstName = "FosterFirst1",
                FosterLastName = "FosterLast1",
                FosterEmail = "FosterEmail1",
                GeneralAge = "Adult",
                EuthanasiaDate = new DateTime(2001, 3, 3),
                EuthanasiaReason = "KillReason1",
                LocationID = "14",
                LocationName = "Location1",
                IsMicrochipped = true,
                Name = "Name1",
                Notes = "Notes1",
                IsOkWithCats = false,
                IsOkWithDogs = true,
                IsOkWithKids = false,
                OlderKidsOnly = true,
                PrimaryBreedID = "15",
                PrimaryBreed = "Breed1",
                ReceivedDate = new DateTime(2001, 4, 4),
                RescueID = "RescueID1",
                Sex = "Sex1",
                HasSpecialDiet = false,
                HasSpecialNeeds = true,
                SpecialNeedsDescription = "SpecialNeedsDescription1",
                Species = "Species1",
                StatusID = "16",
                Status = "Status1",
                LastUpdated = new DateTime(2001, 5, 5, 5, 5, 5)

            };

            CritterSource critterSource2 = new CritterSource()
            {
                ID = 2,
                BirthDate = new DateTime(2002, 1, 1),
                IsBirthDateExact = true,
                ColorID = "22",
                Color = "Color2",
                IsCourtesy = false,
                Created = new DateTime(2002, 2, 2, 2, 2, 2),
                Description = "Description2",
                FosterContactID = "23",
                FosterFirstName = "FosterFirst2",
                FosterLastName = "FosterLast2",
                FosterEmail = "FosterEmail2",
                GeneralAge = "Baby",
                EuthanasiaDate = new DateTime(2002, 3, 3),
                EuthanasiaReason = "KillReason2",
                LocationID = "24",
                LocationName = "Location2",
                IsMicrochipped = true,
                Name = "Name2",
                Notes = "Notes2",
                IsOkWithCats = false,
                IsOkWithDogs = true,
                IsOkWithKids = false,
                OlderKidsOnly = true,
                PrimaryBreedID = "25",
                PrimaryBreed = "Breed2",
                ReceivedDate = new DateTime(2002, 4, 4),
                RescueID = "RescueID2",
                Sex = "Sex2",
                HasSpecialDiet = false,
                HasSpecialNeeds = true,
                SpecialNeedsDescription = "SpecialNeedsDescription2",
                Species = "Species2",
                StatusID = "26",
                Status = "Status2",
                LastUpdated = new DateTime(2002, 5, 5, 5, 5, 5)
            };

            JObject json = JObject.Parse(@"
{
    ""1"": {
        ""animalID"": ""1"",
        ""animalBirthdate"": ""01/01/2001"",
        ""animalBirthdateExact"": ""Yes"",
        ""animalColorID"": ""12"",
        ""animalColor"": ""Color1"",
        ""animalCourtesy"": ""No"",
        ""animalCreatedDate"": ""02/02/2001 2:02:02 AM"",
        ""animalDescription"": ""Description1"",
        ""animalFosterID"": ""13"",
        ""fosterEmail"": ""FosterEmail1"",
        ""fosterFirstname"": ""FosterFirst1"",
        ""fosterLastname"": ""FosterLast1"",
        ""animalGeneralAge"": ""Adult"",
        ""animalKillDate"": ""03/03/2001"",
        ""animalKillReason"": ""KillReason1"",
        ""animalLocationID"": ""14"",
        ""locationName"": ""Location1"",
        ""animalMicrochipped"": ""Yes"",
        ""animalName"": ""Name1"",
        ""animalNotes"": ""Notes1"",
        ""animalOKWithCats"": ""No"",
        ""animalOKWithDogs"": ""Yes"",
        ""animalOKWithKids"": ""No"",
        ""animalOlderKidsOnly"": ""Yes"",
        ""animalPrimaryBreedID"": ""15"",
        ""animalPrimaryBreed"": ""Breed1"",
        ""animalReceivedDate"": ""04/04/2001"",
        ""animalRescueID"": ""RescueID1"",
        ""animalSex"": ""Sex1"",
        ""animalSpecialDiet"": ""No"",
        ""animalSpecialneeds"": ""Yes"",
        ""animalSpecialneedsDescription"": ""SpecialNeedsDescription1"",
        ""animalSpecies"": ""Species1"",
        ""animalStatusID"": ""16"",
        ""animalStatus"": ""Status1"",
        ""animalUpdatedDate"": ""05/05/2001 5:05:05 AM""
    },
    ""2"": {
        ""animalID"": ""2"",
        ""animalBirthdate"": ""01/01/2002"",
        ""animalBirthdateExact"": ""Yes"",
        ""animalColorID"": ""22"",
        ""animalColor"": ""Color2"",
        ""animalCourtesy"": ""No"",
        ""animalCreatedDate"": ""02/02/2002 2:02:02 AM"",
        ""animalDescription"": ""Description2"",
        ""animalFosterID"": ""23"",
        ""fosterEmail"": ""FosterEmail2"",
        ""fosterFirstname"": ""FosterFirst2"",
        ""fosterLastname"": ""FosterLast2"",
        ""animalGeneralAge"": ""Baby"",
        ""animalKillDate"": ""03/03/2002"",
        ""animalKillReason"": ""KillReason2"",
        ""animalLocationID"": ""24"",
        ""locationName"": ""Location2"",
        ""animalMicrochipped"": ""Yes"",
        ""animalName"": ""Name2"",
        ""animalNotes"": ""Notes2"",
        ""animalOKWithCats"": ""No"",
        ""animalOKWithDogs"": ""Yes"",
        ""animalOKWithKids"": ""No"",
        ""animalOlderKidsOnly"": ""Yes"",
        ""animalPrimaryBreedID"": ""25"",
        ""animalPrimaryBreed"": ""Breed2"",
        ""animalReceivedDate"": ""04/04/2002"",
        ""animalRescueID"": ""RescueID2"",
        ""animalSex"": ""Sex2"",
        ""animalSpecialDiet"": ""No"",
        ""animalSpecialneeds"": ""Yes"",
        ""animalSpecialneedsDescription"": ""SpecialNeedsDescription2"",
        ""animalSpecies"": ""Species2"",
        ""animalStatusID"": ""26"",
        ""animalStatus"": ""Status2"",
        ""animalUpdatedDate"": ""05/05/2002 5:05:05 AM""
    }}
");

            IEnumerable<CritterSource> critters = new CritterSourceStorage(new RescueGroupsConfiguration(), null, null).FromStorage(json.Properties());
            critters.Should().HaveCount(2);

            CritterSource result1 = critters.SingleOrDefault(x => x.ID == critterSource1.ID);
            result1.Should().NotBeNull();

            result1.BirthDate.Should().Be(critterSource1.BirthDate);
            result1.IsBirthDateExact.Should().Be(critterSource1.IsBirthDateExact);
            result1.ColorID.Should().Be(critterSource1.ColorID);
            result1.Color.Should().Be(critterSource1.Color);
            result1.IsCourtesy.Should().Be(critterSource1.IsCourtesy);
            result1.Created.Should().Be(critterSource1.Created);
            result1.Description.Should().Be(critterSource1.Description);
            result1.FosterContactID.Should().Be(critterSource1.FosterContactID);
            result1.FosterFirstName.Should().Be(critterSource1.FosterFirstName);
            result1.FosterLastName.Should().Be(critterSource1.FosterLastName);
            result1.FosterEmail.Should().Be(critterSource1.FosterEmail);
            result1.GeneralAge.Should().Be(critterSource1.GeneralAge);
            result1.EuthanasiaDate.Should().Be(critterSource1.EuthanasiaDate);
            result1.EuthanasiaReason.Should().Be(critterSource1.EuthanasiaReason);
            result1.LocationID.Should().Be(critterSource1.LocationID);
            result1.LocationName.Should().Be(critterSource1.LocationName);
            result1.IsMicrochipped.Should().Be(critterSource1.IsMicrochipped);
            result1.Name.Should().Be(critterSource1.Name);
            result1.Notes.Should().Be(critterSource1.Notes);
            result1.IsOkWithCats.Should().Be(critterSource1.IsOkWithCats);
            result1.IsOkWithDogs.Should().Be(critterSource1.IsOkWithDogs);
            result1.IsOkWithKids.Should().Be(critterSource1.IsOkWithKids);
            result1.OlderKidsOnly.Should().Be(critterSource1.OlderKidsOnly);
            result1.PrimaryBreedID.Should().Be(critterSource1.PrimaryBreedID);
            result1.PrimaryBreed.Should().Be(critterSource1.PrimaryBreed);
            result1.ReceivedDate.Should().Be(critterSource1.ReceivedDate);
            result1.RescueID.Should().Be(critterSource1.RescueID);
            result1.Sex.Should().Be(critterSource1.Sex);
            result1.HasSpecialDiet.Should().Be(critterSource1.HasSpecialDiet);
            result1.HasSpecialNeeds.Should().Be(critterSource1.HasSpecialNeeds);
            result1.SpecialNeedsDescription.Should().Be(critterSource1.SpecialNeedsDescription);
            result1.Species.Should().Be(critterSource1.Species);
            result1.StatusID.Should().Be(critterSource1.StatusID);
            result1.Status.Should().Be(critterSource1.Status);
            result1.LastUpdated.Should().Be(critterSource1.LastUpdated);

            CritterSource result2 = critters.SingleOrDefault(x => x.ID == critterSource2.ID);
            result2.Should().NotBeNull();

            result2.BirthDate.Should().Be(critterSource2.BirthDate);
            result2.IsBirthDateExact.Should().Be(critterSource2.IsBirthDateExact);
            result2.ColorID.Should().Be(critterSource2.ColorID);
            result2.Color.Should().Be(critterSource2.Color);
            result2.IsCourtesy.Should().Be(critterSource2.IsCourtesy);
            result2.Created.Should().Be(critterSource2.Created);
            result2.Description.Should().Be(critterSource2.Description);
            result2.FosterContactID.Should().Be(critterSource2.FosterContactID);
            result2.FosterFirstName.Should().Be(critterSource2.FosterFirstName);
            result2.FosterLastName.Should().Be(critterSource2.FosterLastName);
            result2.FosterEmail.Should().Be(critterSource2.FosterEmail);
            result2.GeneralAge.Should().Be(critterSource2.GeneralAge);
            result2.EuthanasiaDate.Should().Be(critterSource2.EuthanasiaDate);
            result2.EuthanasiaReason.Should().Be(critterSource2.EuthanasiaReason);
            result2.LocationID.Should().Be(critterSource2.LocationID);
            result2.LocationName.Should().Be(critterSource2.LocationName);
            result2.IsMicrochipped.Should().Be(critterSource2.IsMicrochipped);
            result2.Name.Should().Be(critterSource2.Name);
            result2.Notes.Should().Be(critterSource2.Notes);
            result2.IsOkWithCats.Should().Be(critterSource2.IsOkWithCats);
            result2.IsOkWithDogs.Should().Be(critterSource2.IsOkWithDogs);
            result2.IsOkWithKids.Should().Be(critterSource2.IsOkWithKids);
            result2.OlderKidsOnly.Should().Be(critterSource2.OlderKidsOnly);
            result2.PrimaryBreedID.Should().Be(critterSource2.PrimaryBreedID);
            result2.PrimaryBreed.Should().Be(critterSource2.PrimaryBreed);
            result2.ReceivedDate.Should().Be(critterSource2.ReceivedDate);
            result2.RescueID.Should().Be(critterSource2.RescueID);
            result2.Sex.Should().Be(critterSource2.Sex);
            result2.HasSpecialDiet.Should().Be(critterSource2.HasSpecialDiet);
            result2.HasSpecialNeeds.Should().Be(critterSource2.HasSpecialNeeds);
            result2.SpecialNeedsDescription.Should().Be(critterSource2.SpecialNeedsDescription);
            result2.Species.Should().Be(critterSource2.Species);
            result2.StatusID.Should().Be(critterSource2.StatusID);
            result2.Status.Should().Be(critterSource2.Status);
            result2.LastUpdated.Should().Be(critterSource2.LastUpdated);
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

            JObject json = JObject.Parse(@"
{
    ""1"": {
        ""animalID"": ""1"",
        ""animalName"": ""Breed 1"",
        ""animalPictures"": [
            {
                ""mediaID"": ""3"",
                ""mediaOrder"": ""44"",
                ""lastUpdated"": ""01/01/2001 1:01 AM"",
                ""fileSize"": ""123456"",
                ""resolutionX"": ""800"",
                ""resolutionY"": ""900"",
                ""fileNameFullsize"": ""Filename1"",
                ""urlSecureFullsize"": ""Url1"",
                ""large"": {
                    ""fileSize"": ""8888"",
                    ""resolutionX"": ""111"",
                    ""resolutionY"": ""222"",
                    ""url"": ""LargeUrl""
                },
                ""small"": {
                    ""fileSize"": ""9999"",
                    ""resolutionX"": ""333"",
                    ""resolutionY"": ""444"",
                    ""url"": ""SmallUrl""
                }
            }
        ]
    }
}
");

            IEnumerable<CritterSource> critters = new CritterSourceStorage(new RescueGroupsConfiguration(), null, null).FromStorage(json.Properties());

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
