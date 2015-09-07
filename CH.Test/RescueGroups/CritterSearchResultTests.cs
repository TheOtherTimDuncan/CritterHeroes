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
    public class CritterSearchResultTests
    {
        [TestMethod]
        public void ObjectTypeIsCorrect()
        {
            new CritterSearchResultStorage(new RescueGroupsConfiguration(), null).ObjectType.Should().Be("animals");
        }

        [TestMethod]
        public void ConvertsJsonResultToModel()
        {
            CritterSearchResult critterSource1 = new CritterSearchResult()
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
                LastUpdated = "01/01/2001",
                Created = "02/02/2002",
                FosterContactID = "4",
                FosterFirstName = "FosterFirstName1",
                FosterLastName = "FosterLastName1",
                FosterEmail = "FosterEmail1"
            };

            CritterSearchResult critterSource2 = new CritterSearchResult()
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
                LastUpdated = "03/03/2003",
                Created = "04/04/2004",
                FosterContactID = "7",
                FosterFirstName = "FosterFirstName2",
                FosterLastName = "FosterLastName2",
                FosterEmail = "FosterEmail2"
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
                new JProperty("animalCreatedDate", critterSource1.Created)
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
                new JProperty("animalCreatedDate", critterSource2.Created)
            ));

            JObject data = new JObject(property1, property2);

            IEnumerable<CritterSearchResult> critters = new CritterSearchResultStorage(new RescueGroupsConfiguration(), null).FromStorage(data.Properties());
            critters.Should().HaveCount(2);

            CritterSearchResult result1 = critters.SingleOrDefault(x => x.ID == critterSource1.ID);
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

            CritterSearchResult result2 = critters.SingleOrDefault(x => x.ID == critterSource2.ID);
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
        }
    }
}
