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

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                storageContext.Entities.MatchingID(person.ID).SingleOrDefault().Should().BeNull();
            }
        }

        [TestMethod]
        public async Task CanCreateReadAndDeletePersonGroup()
        {
            Group group = new Group("person");
            using (SqlStorageContext<Group> storageContext = new SqlStorageContext<Group>())
            {
                storageContext.Add(group);
                await storageContext.SaveChangesAsync();
            }

            Person person = new Person();
            person.AddGroup(group.ID);

            using (SqlStorageContext<Person> storageContext = new SqlStorageContext<Person>())
            {
                storageContext.Add(person);
                await storageContext.SaveChangesAsync();
            }

            using (SqlStorageContext<Group> storageContext = new SqlStorageContext<Group>())
            {
                Group result = await storageContext.Entities.SingleOrDefaultAsync(x => x.ID == group.ID);
                result.Persons.Should().HaveCount(1);

                PersonGroup personGroup = result.Persons.Single();

                personGroup.PersonID.Should().Be(person.ID);
                personGroup.Person.Should().NotBeNull();
                personGroup.Person.ID.Should().Be(person.ID);

                personGroup.GroupID.Should().Be(group.ID);
                personGroup.Group.Should().NotBeNull();
                personGroup.Group.ID.Should().Be(group.ID);

            }

            using (SqlStorageContext<Person> storageContext = new SqlStorageContext<Person>())
            {
                Person result = await storageContext.Entities.FindByIDAsync(person.ID);
                result.Should().NotBeNull();

                result.Groups.Should().HaveCount(1);
                PersonGroup personGroup = result.Groups.Single();

                personGroup.PersonID.Should().Be(person.ID);
                personGroup.Person.Should().NotBeNull();
                personGroup.Person.ID.Should().Be(person.ID);

                personGroup.GroupID.Should().Be(group.ID);
                personGroup.Group.Should().NotBeNull();
                personGroup.Group.ID.Should().Be(group.ID);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                storageContext.Entities.MatchingID(person.ID).SingleOrDefault().Should().BeNull();
            }

            using (SqlStorageContext<Group> storageContext = new SqlStorageContext<Group>())
            {
                Group result = await storageContext.Entities.SingleOrDefaultAsync(x => x.ID == group.ID);

            }
        }
    }
}
