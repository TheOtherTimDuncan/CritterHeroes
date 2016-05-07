using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Mappers;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TOTD.Utility.UnitTestHelpers;

namespace CH.Test.RescueGroups
{
    [TestClass]
    public class CritterMapperTests
    {
        [TestMethod]
        public void CanMapCritterSourceToCritter()
        {
            Guid organizationID = Guid.NewGuid();

            string timeZoneID = "Eastern Standard Time";

            Species species = new Species("species", "singular", "plural");

            Breed breed = new Breed(species, "breed");

            CritterStatus status = new CritterStatus("status", "description");

            CritterColor color = new CritterColor("color")
            {
                RescueGroupsID = 1234
            };

            Person foster = new Person()
            {
                RescueGroupsID = 5678
            };

            Location location = new Location("location")
            {
                RescueGroupsID = 90123
            };

            Critter target = new Critter("critter", status, breed, organizationID);

            CritterSource source = new CritterSource().FillWithTestData();
            source.ColorID = color.RescueGroupsID;
            source.FosterContactID = foster.RescueGroupsID;
            source.LocationID = location.RescueGroupsID;
            source.PrimaryBreedID = breed.RescueGroupsID;
            source.StatusID = status.RescueGroupsID;

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            CritterMapperContext context = new CritterMapperContext(source, target, mockPublisher.Object, timeZoneID)
            {
                Color = color,
                Foster = foster,
                Location = location,
                Breed = breed,
                Status = status
            };

            CritterMapper mapper = new CritterMapper();
            mapper.MapSourceToTarget(context);

            target.BirthDate.Should().Be(source.BirthDate);
            target.IsBirthDateExact.Should().Be(source.IsBirthDateExact);
            target.Color.Should().Be(color);
            target.IsCourtesy.Should().Be(source.IsCourtesy.Value);
            target.RescueGroupsCreated.Should().Be(source.Created);
            target.Description.Should().Be(source.Description);
            target.GeneralAge.Should().Be(source.GeneralAge);
            target.Foster.Should().Be(foster);
            target.EuthanasiaDate.Should().Be(source.EuthanasiaDate);
            target.EuthanasiaReason.Should().Be(source.EuthanasiaReason);
            target.Location.Should().Be(location);
            target.Breed.Should().Be(breed);
            target.IsMicrochipped.Should().Be(source.IsMicrochipped);
            target.Name.Should().Be(source.Name);
            target.Notes.Should().Be(source.Notes);
            target.IsOkWithCats.Should().Be(source.IsOkWithCats);
            target.IsOkWithDogs.Should().Be(source.IsOkWithDogs);
            target.IsOkWithKids.Should().Be(source.IsOkWithKids);
            target.OlderKidsOnly.Should().Be(source.OlderKidsOnly);
            target.BreedID.Should().Be(breed.ID);
            target.ReceivedDate.Should().Be(source.ReceivedDate);
            target.RescueID.Should().Be(source.RescueID);
            target.Sex.Should().Be(source.Sex);
            target.HasSpecialDiet.Should().Be(source.HasSpecialDiet.Value);
            target.HasSpecialNeeds.Should().Be(source.HasSpecialNeeds.Value);
            target.SpecialNeedsDescription.Should().Be(source.SpecialNeedsDescription);
            target.StatusID.Should().Be(status.ID);
            target.RescueGroupsLastUpdated.Should().Be(source.LastUpdated);
        }

        [TestMethod]
        public void CanMapCritterToCritterSource()
        {
            Guid organizationID = Guid.NewGuid();

            string timeZoneID = "Eastern Standard Time";

            Species species = new Species("species", "singular", "plural");

            Breed breed = new Breed(species, "breed")
            {
                RescueGroupsID = 3344
            };

            CritterStatus status = new CritterStatus("status", "description")
            {
                RescueGroupsID = 5544
            };

            CritterColor color = new CritterColor("color")
            {
                RescueGroupsID = 1234
            };

            Person foster = new Person()
            {
                RescueGroupsID = 5678
            };

            Location location = new Location("location")
            {
                RescueGroupsID = 90123
            };

            Critter target = new Critter("critter", status, breed, organizationID, rescueGroupsID: 2233).FillWithTestData();

            target.ChangeColor(color);
            target.ChangeFoster(foster);
            target.ChangeLocation(location);

            CritterSource source = new CritterSource();

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            CritterMapperContext context = new CritterMapperContext(source, target, mockPublisher.Object, timeZoneID)
            {
                Color = color,
                Foster = foster,
                Location = location,
                Breed = breed,
                Status = status
            };

            CritterMapper mapper = new CritterMapper();
            mapper.MapTargetToSource(context);

            source.ID.Should().Be(target.RescueGroupsID);
            source.Name.Should().Be(target.Name);
            source.StatusID.Should().Be(status.RescueGroupsID);
            source.Species.Should().Be(species.Name);
            source.PrimaryBreedID.Should().Be(target.Breed.RescueGroupsID);
            source.Sex.Should().Be(target.Sex);
            source.RescueID.Should().Be(target.RescueID);
            source.FosterContactID.Should().Be(foster.RescueGroupsID);
            source.LocationID.Should().Be(location.RescueGroupsID);
            source.IsCourtesy.Should().Be(target.IsCourtesy);
            source.Description.Should().Be(target.Description);
            source.GeneralAge.Should().Be(target.GeneralAge);
            source.HasSpecialNeeds.Should().Be(target.HasSpecialNeeds);
            source.SpecialNeedsDescription.Should().Be(target.SpecialNeedsDescription);
            source.HasSpecialDiet.Should().Be(target.HasSpecialDiet);
            source.ColorID.Should().Be(color.RescueGroupsID);
            source.BirthDate.Should().Be(target.BirthDate);
            source.IsBirthDateExact.Should().Be(target.IsBirthDateExact);
            source.EuthanasiaDate.Should().Be(target.EuthanasiaDate);
            source.EuthanasiaReason.Should().Be(target.EuthanasiaReason);
            source.Notes.Should().Be(target.Notes);
            source.IsMicrochipped.Should().Be(target.IsMicrochipped);
            source.IsOkWithCats.Should().Be(target.IsOkWithCats);
            source.IsOkWithDogs.Should().Be(target.IsOkWithDogs);
            source.IsOkWithKids.Should().Be(target.IsOkWithKids);
            source.OlderKidsOnly.Should().Be(target.OlderKidsOnly);
        }
    }
}
