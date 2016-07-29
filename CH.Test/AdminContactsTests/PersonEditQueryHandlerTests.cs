using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Features.Admin.Contacts.Models;
using CritterHeroes.Web.Features.Admin.Contacts.Queries;
using CritterHeroes.Web.Features.Shared.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.Utility.UnitTestHelpers;

namespace CH.Test.AdminContactsTests
{
    [TestClass]
    public class PersonEditQueryHandlerTests
    {
        [TestMethod]
        public async Task ReturnsViewModelForEditingPerson()
        {
            State state1 = new State("1", "name1");
            State state2 = new State("2", "name2");

            Person person = new Person()
                .FillWithTestData()
                .SetEntityID(x => x.ID);

            person.State = state1.Abbreviation;

            MockSqlQueryStorageContext<Person> mockPersonStorage = new MockSqlQueryStorageContext<Person>(person);

            MockSqlQueryStorageContext<State> mockStateStorage = new MockSqlQueryStorageContext<State>(state1, state2);

            PersonEditQuery query = new PersonEditQuery()
            {
                PersonID = person.ID
            };

            PersonEditQueryHandler handler = new PersonEditQueryHandler(mockPersonStorage.Object, mockStateStorage.Object);
            PersonEditModel model = await handler.ExecuteAsync(query);
            model.Should().NotBeNull();

            model.PersonID.Should().Be(person.ID);
            model.FirstName.Should().Be(person.FirstName);
            model.LastName.Should().Be(person.LastName);
            model.Email.Should().Be(person.Email);
            model.IsEmailConfirmed.Should().Be(person.IsEmailConfirmed);
            model.Address.Should().Be(person.Address);
            model.City.Should().Be(person.City);
            model.State.Should().Be(person.State);
            model.Zip.Should().Be(person.Zip);

            model.StateOptions.Should().HaveCount(2);

            StateOptionModel option1 = model.StateOptions.SingleOrDefault(x => x.Value == state1.Abbreviation);
            option1.Should().NotBeNull();
            option1.Text.Should().Be(state1.Name);
            option1.IsSelected.Should().BeTrue();

            StateOptionModel option2 = model.StateOptions.SingleOrDefault(x => x.Value == state2.Abbreviation);
            option2.Should().NotBeNull();
            option2.IsSelected.Should().BeFalse();
        }
    }
}
