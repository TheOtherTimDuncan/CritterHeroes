using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using CritterHeroes.Web.Domain.Contracts.Events;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.RescueGroups
{
    [TestClass]
    public class PersonSourceTests
    {
        [TestMethod]
        public void ObjectTypeIsCorrect()
        {
            new PersonSourceStorage(new RescueGroupsConfiguration(), null, null).ObjectType.Should().Be("contacts");
        }

        [TestMethod]
        public void CanSplitExtendedZipIntoPostalCodeAndPlus4()
        {
            PersonSource source = new PersonSource();
            source.Zip = "123456789";
            source.PostalCode.Should().Be("12345");
            source.PostalPlus4.Should().Be("6789");
        }

        [TestMethod]
        public void SetsPostalCodeToShortZip()
        {
            PersonSource source = new PersonSource();
            source.Zip = "12345";
            source.PostalCode.Should().Be("12345");
            source.PostalPlus4.Should().BeNull();
        }

        [TestMethod]
        public void CombinesPostalCodeAndPlus4IntoZip()
        {
            PersonSource source = new PersonSource();
            source.PostalCode = "12345";
            source.PostalPlus4 = "6789";
            source.Zip.Should().Be("123456789");
        }

        [TestMethod]
        public async Task ConvertsJsonToModel()
        {
            PersonSource source1 = new PersonSource()
            {
                ID = 1,
                FirstName = "First1",
                LastName = "Last1",
                Email = "email1@email1.com",
                Address = "Address1",
                City = "City1",
                State = "OH1",
                PostalCode = "Zip1",
                PostalPlus4 = "1234",
                Groups = "group1,group2",
                PhoneHome = "123456",
                PhoneWork = "789123",
                PhoneWorkExtension = "WorkExt",
                PhoneCell = "1122",
                PhoneFax = "3344",
                IsActive = true
            };

            source1.GroupNames.Should().Equal(new[] { "group1", "group2" });

            PersonSource source2 = new PersonSource()
            {
                ID = 2,
                FirstName = "First2",
                LastName = "Last2",
                Email = "email2@email2.com",
                Address = "Address2",
                City = "City2",
                State = "OH2",
                PostalCode = "Zip2",
                PostalPlus4 = "5678",
                IsActive = false
            };

            string json = @"
{
    ""1"": {
        ""contactID"": ""1"",
        ""contactClass"": ""Individual/Family"",
        ""contactFirstname"": ""First1"",
        ""contactLastname"": ""Last1"",
        ""contactAddress"": ""Address1"",
        ""contactCity"": ""City1"",
        ""contactState"": ""OH1"",
        ""contactPostalcode"": ""Zip1"",
        ""contactPlus4"": ""1234"",
        ""contactEmail"": ""email1@email1.com"",
        ""contactPhoneHome"": ""(123) 456"",
        ""contactPhoneWork"": ""789-123"",
        ""contactPhoneWorkExt"": ""WorkExt"",
        ""contactPhoneCell"": ""11.22"",
        ""contactFax"": ""33 44"",
        ""contactActive"": ""Yes"",
        ""contactGroups"": ""group1,group2""
    },
    ""2"": {
       ""contactID"": ""2"",
        ""contactClass"": ""Individual/Family"",
        ""contactFirstname"": ""First2"",
        ""contactLastname"": ""Last2"",
        ""contactAddress"": ""Address2"",
        ""contactCity"": ""City2"",
        ""contactState"": ""OH2"",
        ""contactPostalcode"": ""Zip2"",
        ""contactPlus4"": ""5678"",
        ""contactEmail"": ""email2@email2.com"",
        ""contactPhoneHome"": """",
        ""contactPhoneWork"": """",
        ""contactPhoneWorkExt"": """",
        ""contactPhoneCell"": """",
        ""contactFax"": """",
        ""contactActive"": ""No"",
        ""contactGroups"": """"
    }}
";

            MockHttpClient mockClient = new MockHttpClient()
             .SetupLoginResponse()
             .SetupSearchResponse(json);

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            PersonSourceStorage storage = new PersonSourceStorage(new RescueGroupsConfiguration(), mockClient.Object, mockPublisher.Object);

            IEnumerable<PersonSource> results = await storage.GetAllAsync();
            results.Should().HaveCount(2);

            PersonSource result1 = results.SingleOrDefault(x => x.ID == source1.ID);
            result1.Should().NotBeNull();

            result1.FirstName.Should().Be(source1.FirstName);
            result1.LastName.Should().Be(source1.LastName);
            result1.Email.Should().Be(source1.Email);
            result1.Address.Should().Be(source1.Address);
            result1.City.Should().Be(source1.City);
            result1.State.Should().Be(source1.State);
            result1.Zip.Should().Be(source1.Zip);
            result1.PhoneHome.Should().Be(source1.PhoneHome);
            result1.PhoneWork.Should().Be(source1.PhoneWork);
            result1.PhoneWorkExtension.Should().Be(source1.PhoneWorkExtension);
            result1.PhoneCell.Should().Be(source1.PhoneCell);
            result1.PhoneFax.Should().Be(source1.PhoneFax);
            result1.IsActive.Should().Be(source1.IsActive);
            result1.GroupNames.Should().Equal(source1.GroupNames);

            PersonSource result2 = results.SingleOrDefault(x => x.ID == source2.ID);
            result2.Should().NotBeNull();

            result2.FirstName.Should().Be(source2.FirstName);
            result2.LastName.Should().Be(source2.LastName);
            result2.Email.Should().Be(source2.Email);
            result2.Address.Should().Be(source2.Address);
            result2.City.Should().Be(source2.City);
            result2.State.Should().Be(source2.State);
            result2.Zip.Should().Be(source2.Zip);
            result2.PhoneHome.Should().Be(source2.PhoneHome);
            result2.PhoneWork.Should().Be(source2.PhoneWork);
            result2.PhoneWorkExtension.Should().Be(source2.PhoneWorkExtension);
            result2.PhoneCell.Should().Be(source2.PhoneCell);
            result2.PhoneFax.Should().Be(source2.PhoneFax);
            result2.IsActive.Should().Be(source2.IsActive);
            result2.GroupNames.Should().BeNullOrEmpty();
        }
    }
}
