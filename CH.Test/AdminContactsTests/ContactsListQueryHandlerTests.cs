using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Features.Admin.Contacts.Models;
using CritterHeroes.Web.Features.Admin.Contacts.Queries;
using EntityFramework.Testing;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TOTD.Utility.UnitTestHelpers;

namespace CH.Test.AdminContactsTests
{
    [TestClass]
    public class ContactsListQueryHandlerTests
    {
        private IEnumerable<Person> testPeople;
        private IEnumerable<Business> testBusinesses;

        private Group group1;
        private Group group2;

        private Mock<IContactsStorageContext> mockContactsStorage;

        [TestInitialize]
        public void InitializeTestData()
        {
            group1 = new Group("group1").SetEntityID(x => x.ID);
            group2 = new Group("group2").SetEntityID(x => x.ID);

            testPeople = Enumerable.Range(1, 10).Select(r =>
            {
                Person person = new Person()
                {
                    FirstName = Faker.Name.First(),
                    LastName = Faker.Name.Last(),
                    Address = Faker.Address.StreetAddress(),
                    City = Faker.Address.City(),
                    State = Faker.Address.UsStateAbbr(),
                    Zip = Faker.Address.ZipCode(),
                    Email = Faker.Internet.Email()

                }.SetEntityID(x => x.ID);

                if (r <= 5)
                {
                    person.AddGroup(group1);
                }
                else
                {
                    person.AddGroup(group2);
                }

                person.AddPhoneNumber(Faker.Phone.Number(), null, 1);

                return person;
            }).ToList();

            testBusinesses = Enumerable.Range(1, 10).Select(r =>
            {
                Business business = new Business()
                {
                    Name = Faker.Company.Name(),
                    Address = Faker.Address.StreetAddress(),
                    City = Faker.Address.City(),
                    State = Faker.Address.UsStateAbbr(),
                    Zip = Faker.Address.ZipCode(),
                    Email = Faker.Internet.Email()
                }.SetEntityID(x => x.ID);

                if (r <= 5)
                {
                    business.AddGroup(group1);
                }
                else
                {
                    business.AddGroup(group2);
                }

                return business;
            }).ToList();

            mockContactsStorage = new Mock<IContactsStorageContext>();
            mockContactsStorage.Setup(x => x.People).Returns(new TestDbAsyncEnumerable<Person>(testPeople));
            mockContactsStorage.Setup(x => x.Businesses).Returns(new TestDbAsyncEnumerable<Business>(testBusinesses));
        }

        [TestMethod]
        public async Task ReturnsViewModelWithListOfPeopleAndBusinesses()
        {
            ContactsListQuery query = new ContactsListQuery()
            {
                Show = ContactsQuery.ShowKeys.All
            };

            ContactsListQueryHandler handler = new ContactsListQueryHandler(mockContactsStorage.Object);
            ContactsListModel model = await handler.ExecuteAsync(query);

            model.Paging.Should().NotBeNull();

            foreach (Person person in testPeople)
            {
                ContactModel contactModel = model.Contacts.SingleOrDefault(x => x.ContactID == person.ID);
                contactModel.Should().NotBeNull();
                contactModel.ContactName.Should().Be($"{person.LastName}, {person.FirstName}");
                contactModel.Address.Should().Be(person.Address);
                contactModel.City.Should().Be(person.City);
                contactModel.State.Should().Be(person.State);
                contactModel.Zip.Should().Be(person.Zip);
                contactModel.Email.Should().Be(person.Email);
                contactModel.Groups.Should().Be(person.Groups.Single().Group.Name);
                contactModel.IsActive.Should().Be(person.IsActive);
                contactModel.IsPerson.Should().BeTrue();
                contactModel.IsBusiness.Should().BeFalse();
            }

            foreach (Business business in testBusinesses)
            {
                ContactModel contactModel = model.Contacts.SingleOrDefault(x => x.ContactID == business.ID);
                contactModel.Should().NotBeNull();
                contactModel.ContactName.Should().Be(business.Name);
                contactModel.Address.Should().Be(business.Address);
                contactModel.City.Should().Be(business.City);
                contactModel.State.Should().Be(business.State);
                contactModel.Zip.Should().Be(business.Zip);
                contactModel.Email.Should().Be(business.Email);
                contactModel.Groups.Should().Be(business.Groups.Single().Group.Name);
                contactModel.IsActive.Should().BeTrue();
                contactModel.IsPerson.Should().BeFalse();
                contactModel.IsBusiness.Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task ReturnsViewModelWithOnlyPeopleIfBusinessesExcluded()
        {
            ContactsListQuery query = new ContactsListQuery()
            {
                Show = ContactsQuery.ShowKeys.People
            };

            ContactsListQueryHandler handler = new ContactsListQueryHandler(mockContactsStorage.Object);
            ContactsListModel model = await handler.ExecuteAsync(query);

            model.Paging.Should().NotBeNull();

            model.Contacts.Select(x => x.ContactID).Should().Contain(testPeople.Select(x => x.ID));
            model.Contacts.Select(x => x.ContactID).Should().NotContain(testBusinesses.Select(x => x.ID));
        }

        [TestMethod]
        public async Task ReturnsViewModelWithOnlyBusinessesIfPeopleExcluded()
        {
            ContactsListQuery query = new ContactsListQuery()
            {
                Show = ContactsQuery.ShowKeys.Businesses
            };

            ContactsListQueryHandler handler = new ContactsListQueryHandler(mockContactsStorage.Object);
            ContactsListModel model = await handler.ExecuteAsync(query);

            model.Paging.Should().NotBeNull();

            model.Contacts.Select(x => x.ContactID).Should().NotContain(testPeople.Select(x => x.ID));
            model.Contacts.Select(x => x.ContactID).Should().Contain(testBusinesses.Select(x => x.ID));
        }

        [TestMethod]
        public async Task ReturnsViewModelWithOnlyActivePeopleIfInactivesExcluded()
        {
            ContactsListQuery query = new ContactsListQuery()
            {
                Show = ContactsQuery.ShowKeys.All,
                Status = ContactsQuery.StatusKeys.Active
            };

            testPeople.All(x => x.IsActive == true).Should().BeTrue();

            Person person = testPeople.First();
            person.IsActive = false;

            ContactsListQueryHandler handler = new ContactsListQueryHandler(mockContactsStorage.Object);
            ContactsListModel model = await handler.ExecuteAsync(query);

            model.Paging.Should().NotBeNull();

            model.Contacts.Select(x => x.ContactID).Should()
                .Contain(testPeople.Where(x => x.IsActive).Select(x => x.ID))
                .And
                .NotContain(person.ID);

            model.Contacts.Select(x => x.ContactID).Should().Contain(testBusinesses.Select(x => x.ID));
        }

        [TestMethod]
        public async Task ReturnsViewModelWithOnlyInaActivePeopleIfActivesExcluded()
        {
            ContactsListQuery query = new ContactsListQuery()
            {
                Show = ContactsQuery.ShowKeys.All,
                Status = ContactsQuery.StatusKeys.Inactive
            };

            Person person = testPeople.First();
            person.IsActive = false;

            ContactsListQueryHandler handler = new ContactsListQueryHandler(mockContactsStorage.Object);
            ContactsListModel model = await handler.ExecuteAsync(query);

            model.Paging.Should().NotBeNull();

            model.Contacts.Select(x => x.ContactID).Should()
                .NotContain(testPeople.Where(x => !x.IsActive).Select(x => x.ID))
                .And
                .Contain(person.ID);

            model.Contacts.Select(x => x.ContactID).Should().Contain(testBusinesses.Select(x => x.ID));
        }

        [TestMethod]
        public async Task ReturnsViewModelWithOnlyPeopleAndBusinessesInSpecifiedGroup()
        {
            ContactsListQuery query = new ContactsListQuery()
            {
                Show = ContactsQuery.ShowKeys.All,
                GroupID = group1.ID
            };

            ContactsListQueryHandler handler = new ContactsListQueryHandler(mockContactsStorage.Object);
            ContactsListModel model = await handler.ExecuteAsync(query);

            model.Paging.Should().NotBeNull();

            model.Contacts.Select(x => x.ContactID).Should()
                .Contain(testPeople.Where(x => x.Groups.Any(g => g.GroupID == query.GroupID)).Select(x=>x.ID))
                .And
                .NotContain(testPeople.Where(x => x.Groups.Any(g => g.GroupID != query.GroupID)).Select(x => x.ID));

            model.Contacts.Select(x => x.ContactID).Should()
                .Contain(testBusinesses.Where(x => x.Groups.Any(g => g.GroupID == query.GroupID)).Select(x => x.ID))
                .And
                .NotContain(testBusinesses.Where(x => x.Groups.Any(g => g.GroupID != query.GroupID)).Select(x => x.ID));
        }
    }
}
