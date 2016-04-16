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

namespace CH.Test.RescueGroups
{
    [TestClass]
    public class CritterMapperTests
    {
        [TestMethod]
        public void CanMapCritterSourceToCritter()
        {
            Guid organizationID = Guid.NewGuid();

            Species species = new Species("species", "singular", "plural");

            Breed breed = new Breed(species, "breed");

            CritterStatus status = new CritterStatus("status", "description");

            CritterColor color = new CritterColor("color")
            {
                RescueGroupsID = "colorid"
            };

            Person foster = new Person()
            {
                RescueGroupsID = "personid"
            };

            Location location = new Location("location")
            {
                RescueGroupsID = "locationid"
            };

            Critter target = new Critter("critter", status, breed, organizationID);

            CritterSource source = new CritterSource()
            {
                BirthDate = new DateTime(2001, 1, 1),
                IsBirthDateExact = true,
                ColorID = color.RescueGroupsID,
                IsCourtesy = false,
                Created = new DateTime(2001, 2, 2),
                Description = "description",
                FosterContactID = foster.RescueGroupsID,
                GeneralAge = "generalage",
                EuthanasiaDate = new DateTime(2001, 3, 3),
                EuthanasiaReason = "euthanisiareason",
                LocationID = location.RescueGroupsID,
                IsMicrochipped = true,
                Name = "name",
                Notes = "notes",
                IsOkWithCats = false,
                IsOkWithDogs = true,
                IsOkWithKids = false,
                OlderKidsOnly = true,
                PrimaryBreedID = breed.RescueGroupsID,
                ReceivedDate = new DateTime(2001, 4, 4),
                RescueID = "rescueuid",
                Sex = "sex",
                HasSpecialDiet = false,
                HasSpecialNeeds = true,
                SpecialNeedsDescription = "specialneeds",
                Status = status.RescueGroupsID,
                LastUpdated = new DateTime(2001, 5, 5)
            };

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            CritterMapperContext context = new CritterMapperContext(source, target, mockPublisher.Object)
            {
                Color = color,
                Foster = foster,
                Location = location,
                Breed = breed,
                Status = status
            };

            CritterMapper mapper = new CritterMapper();
            mapper.MapTo(context);

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
    }
}
