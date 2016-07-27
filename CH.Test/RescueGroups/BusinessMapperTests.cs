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
    public class BusinessMapperTests
    {
        [TestMethod]
        public void CanMapBusinessSourceToBusiness()
        {
            Group group1 = new Group("group1").SetEntityID(x => x.ID);
            Group group2 = new Group("group2").SetEntityID(x => x.ID);

            Business target = new Business();

            BusinessSource source = new BusinessSource().FillWithTestData("Zip");
            source.Groups = $"{group1.Name},{group2.Name}";

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            BusinessMapperContext context = new BusinessMapperContext(source, target, mockPublisher.Object)
            {
                Groups = new[] { group1, group2 },

                PhoneTypes = PhoneTypeNames.GetAll()
                    .Select(x => new PhoneType(x).SetEntityID(e => e.ID))
                    .ToList()
            };

            BusinessMapper mapper = new BusinessMapper();
            mapper.MapSourceToTarget(context);

            target.Name.Should().Be(source.Company);
            target.Email.Should().Be(source.Email);
            target.Address.Should().Be(source.Address);
            target.City.Should().Be(source.City);
            target.State.Should().Be(source.State);
            target.Zip.Should().Be(source.Zip);
            target.RescueGroupsID.Should().Be(source.ID);

            BusinessPhone phoneFax = target.PhoneNumbers.SingleOrDefault(x => x.PhoneTypeID == context.PhoneTypeFax.ID);
            phoneFax.Should().NotBeNull();
            phoneFax.PhoneNumber.Should().Be(source.PhoneFax);

            BusinessPhone phoneWork = target.PhoneNumbers.SingleOrDefault(x => x.PhoneTypeID == context.PhoneTypeWork.ID);
            phoneWork.Should().NotBeNull();
            phoneWork.PhoneNumber.Should().Be(source.PhoneWork);
            phoneWork.PhoneExtension.Should().Be(source.PhoneWorkExtension);

            target.Groups.Should().HaveCount(2);
            target.Groups.Should().Contain(x => x.GroupID == group1.ID);
            target.Groups.Should().Contain(x => x.GroupID == group2.ID);
        }

        [TestMethod]
        public void CanMapBusinessToBusinessSource()
        {
            Group group1 = new Group("group1").SetEntityID(x => x.ID);
            Group group2 = new Group("group2").SetEntityID(x => x.ID);

            Business target = new Business().FillWithTestData();
            target.Zip = "123456789";

            BusinessSource source = new BusinessSource();

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            BusinessMapperContext context = new BusinessMapperContext(source, target, mockPublisher.Object)
            {
                Groups = new[] { group1, group2 },

                PhoneTypes = PhoneTypeNames.GetAll()
                    .Select(x => new PhoneType(x).SetEntityID(e => e.ID))
                    .ToList()
            };

            BusinessPhone phoneFax = target.AddPhoneNumber("fax", null, context.PhoneTypeFax.ID);
            BusinessPhone phoneWork = target.AddPhoneNumber("work", "ext", context.PhoneTypeWork.ID);

            BusinessMapper mapper = new BusinessMapper();
            mapper.MapTargetToSource(context);

            source.Company.Should().Be(target.Name);
            source.Email.Should().Be(target.Email);
            source.Address.Should().Be(target.Address);
            source.City.Should().Be(source.City);
            source.State.Should().Be(target.State);
            source.Zip.Should().Be(target.Zip);
            source.ID.Should().Be(target.RescueGroupsID);
            source.PhoneFax.Should().Be(phoneFax.PhoneNumber);
            source.PhoneWork.Should().Be(phoneWork.PhoneNumber);
            source.PhoneWorkExtension.Should().Be(phoneWork.PhoneExtension);
        }
    }
}
