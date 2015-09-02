using System;
using System.Collections.Generic;
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
    public class PersonTests : BaseTest
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
    }
}
