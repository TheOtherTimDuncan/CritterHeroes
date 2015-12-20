using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Contexts;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.EntityFramework;

namespace CH.Test.EntityTests
{
    [TestClass]
    public class PersonTests : BaseEntityTest
    {
        [TestMethod]
        public async Task CanCreateReadAndDeletePerson()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            State state = new State("OH", "Ohio");
            using (SqlStorageContext<State> storageContext = new SqlStorageContext<State>())
            {
                storageContext.Add(state);
                await storageContext.SaveChangesAsync();
            }

            Person person = new Person()
            {
                State = state.Abbreviation
            };

            using (SqlStorageContext<Person> storageContext = new SqlStorageContext<Person>())
            {
                EntityTestHelper.FillWithTestData(storageContext, person, "ID", "State");
                storageContext.Add(person);
                await storageContext.SaveChangesAsync();
            }

            using (SqlStorageContext<Person> storageContext = new SqlStorageContext<Person>())
            {
                Person result = await storageContext.Entities.FindByIDAsync(person.ID);
                result.Should().NotBeNull();

                result.FirstName.Should().Be(person.FirstName);
                result.LastName.Should().Be(person.LastName);
                result.Email.Should().Be(person.Email);
                result.Address.Should().Be(person.Address);
                result.City.Should().Be(person.City);
                result.Zip.Should().Be(person.Zip);
                result.State.Should().Be(person.State);
                result.RescueGroupsID.Should().Be(person.RescueGroupsID);
                result.IsActive.Should().Be(person.IsActive);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                storageContext.Entities.MatchingID(person.ID).SingleOrDefault().Should().BeNull();
            }
        }

        [TestMethod]
        public async Task CanCreateReadAndDeletePersonGroup()
        {
            Group group1 = new Group("person1");
            Group group2 = new Group("person2");

            Person person = new Person();
            person.AddGroup(group1);
            person.AddGroup(group2);

            using (SqlStorageContext<Person> storageContext = new SqlStorageContext<Person>())
            {
                storageContext.Add(person);
                await storageContext.SaveChangesAsync();
            }

            using (SqlStorageContext<Person> storageContext = new SqlStorageContext<Person>())
            {
                Person result = await storageContext.Entities.FindByIDAsync(person.ID);
                result.Should().NotBeNull();

                result.Groups.Should().HaveCount(2);

                PersonGroup personGroup1 = result.Groups.SingleOrDefault(x => x.GroupID == group1.ID);
                personGroup1.PersonID.Should().Be(person.ID);
                personGroup1.Person.Should().NotBeNull();
                personGroup1.Person.ID.Should().Be(person.ID);
                personGroup1.Group.Should().NotBeNull();
                personGroup1.Group.ID.Should().Be(group1.ID);

                PersonGroup personGroup2 = result.Groups.SingleOrDefault(x => x.GroupID == group2.ID);
                personGroup2.PersonID.Should().Be(person.ID);
                personGroup2.Person.Should().NotBeNull();
                personGroup2.Person.ID.Should().Be(person.ID);
                personGroup2.Group.Should().NotBeNull();
                personGroup2.Group.ID.Should().Be(group2.ID);

                // Can group be removed from person?
                result.Groups.Remove(personGroup1);
                await storageContext.SaveChangesAsync();

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                storageContext.Entities.MatchingID(person.ID).SingleOrDefault().Should().BeNull();
            }

            using (SqlStorageContext<Group> storageContext = new SqlStorageContext<Group>())
            {
                Group result1 = await storageContext.Entities.SingleOrDefaultAsync(x => x.ID == group1.ID);
                result1.Should().NotBeNull("group should still exist after person linked to group is deleted");

            }
        }

        [TestMethod]
        public async Task CanReadWriteAndDeletePersonPhone()
        {
            PhoneType phoneType = new PhoneType("persontest");
            using (SqlStorageContext<PhoneType> storageContext = new SqlStorageContext<PhoneType>())
            {
                storageContext.Add(phoneType);
                await storageContext.SaveChangesAsync();
            }

            Person person = new Person();
            PersonPhone personPhone1 = person.AddPhoneNumber("1234567890", "123456", phoneType);
            PersonPhone personPhone2 = person.AddPhoneNumber("9876543210", null, phoneType);

            using (SqlStorageContext<Person> storageContext = new SqlStorageContext<Person>())
            {
                storageContext.Add(person);
                await storageContext.SaveChangesAsync();
            }

            using (SqlStorageContext<Person> storageContext = new SqlStorageContext<Person>())
            {
                Person result = await storageContext.Entities.FindByIDAsync(person.ID);
                result.Should().NotBeNull();

                result.PhoneNumbers.Should().HaveCount(2);

                PersonPhone resultPhone = result.PhoneNumbers.Single(x => x.PhoneNumber == personPhone1.PhoneNumber);

                resultPhone.PersonID.Should().Be(person.ID);
                resultPhone.Person.Should().NotBeNull();
                resultPhone.Person.ID.Should().Be(person.ID);

                resultPhone.PhoneTypeID.Should().Be(phoneType.ID);
                resultPhone.PhoneType.Should().NotBeNull();
                resultPhone.PhoneType.ID.Should().Be(phoneType.ID);

                resultPhone.PhoneExtension.Should().Be(personPhone1.PhoneExtension);

                // Can phone be removed from person?
                result.PhoneNumbers.Remove(resultPhone);
                await storageContext.SaveChangesAsync();

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();
            }
        }
    }
}
