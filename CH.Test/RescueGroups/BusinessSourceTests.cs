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
    public class BusinessSourceTests
    {
        [TestMethod]
        public void ObjectTypeIsCorrect()
        {
            new BusinessSourceStorage(new RescueGroupsConfiguration(), null, null).ObjectType.Should().Be("contacts");
        }

        [TestMethod]
        public async Task ConvertsJsonToModel()
        {
            BusinessSource source1 = new BusinessSource()
            {
                ID = 1,
                Name = "Name1",
                Company = "Company1",
                Email = "email1@email1.com",
                Address = "Address1",
                City = "City1",
                State = "OH1",
                PostalCode = "Zip1",
                PostalPlus4 = "1234",
                Groups = "group1,group2",
                PhoneWork = "789123",
                PhoneWorkExtension = "WorkExt",
                PhoneFax = "3344"
            };

            source1.GroupNames.Should().Equal(new[] { "group1", "group2" });
            source1.Zip.Should().Be("Zip11234");

            BusinessSource source2 = new BusinessSource()
            {
                ID = 2,
                Name = "Name2",
                Company = "Company2",
                Email = "email2@email2.com",
                Address = "Address2",
                City = "City2",
                State = "OH2",
                PostalCode = "Zip2",
                PostalPlus4 = "5678"
            };

            string json = @"
{
    ""1"": {
        ""contactID"": ""1"",
        ""contactClass"": ""Individual/Family"",
        ""contactName"": ""Name1"",
        ""contactCompany"": ""Company1"",
        ""contactAddress"": ""Address1"",
        ""contactCity"": ""City1"",
        ""contactState"": ""OH1"",
        ""contactPostalcode"": ""Zip1"",
        ""contactPlus4"": ""1234"",
        ""contactEmail"": ""email1@email1.com"",
        ""contactPhoneWork"": ""789-123"",
        ""contactPhoneWorkExt"": ""WorkExt"",
        ""contactFax"": ""33 44"",
        ""contactGroups"": ""group1,group2""
    },
    ""2"": {
       ""contactID"": ""2"",
        ""contactClass"": ""Individual/Family"",
        ""contactName"": ""Name2"",
        ""contactCompany"": ""Company2"",
        ""contactAddress"": ""Address2"",
        ""contactCity"": ""City2"",
        ""contactState"": ""OH2"",
        ""contactPostalcode"": ""Zip2"",
        ""contactPlus4"": ""5678"",
        ""contactEmail"": ""email2@email2.com"",
        ""contactPhoneWork"": """",
        ""contactPhoneWorkExt"": """",
        ""contactFax"": """",
        ""contactGroups"": """"
    }}
";

            MockHttpClient mockClient = new MockHttpClient()
             .SetupLoginResponse()
             .SetupSearchResponse(json);

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            BusinessSourceStorage storage = new BusinessSourceStorage(new RescueGroupsConfiguration(), mockClient.Object, mockPublisher.Object);

            IEnumerable<BusinessSource> results = await storage.GetAllAsync();
            results.Should().HaveCount(2);

            BusinessSource result1 = results.SingleOrDefault(x => x.ID == source1.ID);
            result1.Should().NotBeNull();

            result1.Name.Should().Be(source1.Name);
            result1.Company.Should().Be(source1.Company);
            result1.Email.Should().Be(source1.Email);
            result1.Address.Should().Be(source1.Address);
            result1.City.Should().Be(source1.City);
            result1.State.Should().Be(source1.State);
            result1.Zip.Should().Be(source1.Zip);
            result1.PhoneWork.Should().Be(source1.PhoneWork);
            result1.PhoneWorkExtension.Should().Be(source1.PhoneWorkExtension);
            result1.PhoneFax.Should().Be(source1.PhoneFax);
            result1.GroupNames.Should().Equal(source1.GroupNames);

            BusinessSource result2 = results.SingleOrDefault(x => x.ID == source2.ID);
            result2.Should().NotBeNull();

            result2.Name.Should().Be(source2.Name);
            result2.Company.Should().Be(source2.Company);
            result2.Email.Should().Be(source2.Email);
            result2.Address.Should().Be(source2.Address);
            result2.City.Should().Be(source2.City);
            result2.State.Should().Be(source2.State);
            result2.Zip.Should().Be(source2.Zip);
            result2.PhoneWork.Should().Be(source2.PhoneWork);
            result2.PhoneWorkExtension.Should().Be(source2.PhoneWorkExtension);
            result2.PhoneFax.Should().Be(source2.PhoneFax);
            result2.GroupNames.Should().BeNullOrEmpty();
        }
    }
}
