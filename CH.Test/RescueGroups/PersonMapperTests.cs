using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Mappers;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.Domain.Contracts.Events;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TOTD.Utility.UnitTestHelpers;

namespace CH.Test.RescueGroups
{
    [TestClass]
    public class PersonMapperTests
    {
        [TestMethod]
        public void CanMapPersonSourceToPerson()
        {
            Group group1 = new Group("group1").SetEntityID(x => x.ID);
            Group group2 = new Group("group2").SetEntityID(x => x.ID);

            Person target = new Person();

            PersonSource source = new PersonSource().FillWithTestData("Zip");
            source.Groups = $"{group1.Name},{group2.Name}";

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            PersonMapperContext context = new PersonMapperContext(source, target, mockPublisher.Object)
            {
                Groups = new[] { group1, group2 },

                PhoneTypes = PhoneTypeNames.GetAll()
                    .Select(x => new PhoneType(x).SetEntityID(e => e.ID))
                    .ToList()
            };

            PersonMapper mapper = new PersonMapper();
            mapper.MapSourceToTarget(context);

            target.FirstName.Should().Be(source.FirstName);
            target.LastName.Should().Be(source.LastName);
            target.Email.Should().Be(source.Email);
            target.Address.Should().Be(source.Address);
            target.City.Should().Be(source.City);
            target.State.Should().Be(source.State);
            target.Zip.Should().Be(source.Zip);
            target.RescueGroupsID.Should().Be(source.ID);
            target.IsActive.Should().Be(source.IsActive);

            PersonPhone phoneHome = target.PhoneNumbers.SingleOrDefault(x => x.PhoneTypeID == context.PhoneTypeHome.ID);
            phoneHome.Should().NotBeNull();
            phoneHome.PhoneNumber.Should().Be(source.PhoneHome);

            PersonPhone phoneCell = target.PhoneNumbers.SingleOrDefault(x => x.PhoneTypeID == context.PhoneTypeCell.ID);
            phoneCell.Should().NotBeNull();
            phoneCell.PhoneNumber.Should().Be(source.PhoneCell);

            PersonPhone phoneFax = target.PhoneNumbers.SingleOrDefault(x => x.PhoneTypeID == context.PhoneTypeFax.ID);
            phoneFax.Should().NotBeNull();
            phoneFax.PhoneNumber.Should().Be(source.PhoneFax);

            PersonPhone phoneWork = target.PhoneNumbers.SingleOrDefault(x => x.PhoneTypeID == context.PhoneTypeWork.ID);
            phoneWork.Should().NotBeNull();
            phoneWork.PhoneNumber.Should().Be(source.PhoneWork);
            phoneWork.PhoneExtension.Should().Be(source.PhoneWorkExtension);

            target.Groups.Should().HaveCount(2);
            target.Groups.Should().Contain(x => x.GroupID == group1.ID);
            target.Groups.Should().Contain(x => x.GroupID == group2.ID);
        }

        [TestMethod]
        public void CanMapPersonToPersonSource()
        {
            Group group1 = new Group("group1").SetEntityID(x => x.ID);
            Group group2 = new Group("group2").SetEntityID(x => x.ID);

            Person target = new Person().FillWithTestData();
            target.Zip = "123456789";

            PersonSource source = new PersonSource();

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            PersonMapperContext context = new PersonMapperContext(source, target, mockPublisher.Object)
            {
                Groups = new[] { group1, group2 },

                PhoneTypes = PhoneTypeNames.GetAll()
                    .Select(x => new PhoneType(x).SetEntityID(e => e.ID))
                    .ToList()
            };

            PersonPhone phoneHome = target.AddPhoneNumber("home", null, context.PhoneTypeHome.ID);
            PersonPhone phoneCell = target.AddPhoneNumber("cell", null, context.PhoneTypeCell.ID);
            PersonPhone phoneFax = target.AddPhoneNumber("fax", null, context.PhoneTypeFax.ID);
            PersonPhone phoneWork = target.AddPhoneNumber("work", "ext", context.PhoneTypeWork.ID);

            PersonMapper mapper = new PersonMapper();
            mapper.MapTargetToSource(context);

            source.FirstName.Should().Be(target.FirstName);
            source.LastName.Should().Be(target.LastName);
            source.Email.Should().Be(target.Email);
            source.Address.Should().Be(target.Address);
            source.City.Should().Be(source.City);
            source.State.Should().Be(target.State);
            source.Zip.Should().Be(target.Zip);
            source.ID.Should().Be(target.RescueGroupsID);
            source.IsActive.Should().Be(target.IsActive);
            source.PhoneHome.Should().Be(phoneHome.PhoneNumber);
            source.PhoneCell.Should().Be(phoneCell.PhoneNumber);
            source.PhoneFax.Should().Be(phoneFax.PhoneNumber);
            source.PhoneWork.Should().Be(phoneWork.PhoneNumber);
            source.PhoneWorkExtension.Should().Be(phoneWork.PhoneExtension);
        }
    }
}
