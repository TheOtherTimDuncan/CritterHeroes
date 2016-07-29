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
    public class BusinessEditQueryHandlerTests
    {
        [TestMethod]
        public async Task ReturnsViewModelForEditingBusiness()
        {
            State state1 = new State("1", "name1");
            State state2 = new State("2", "name2");

            Business business = new Business()
                .FillWithTestData()
                .SetEntityID(x => x.ID);

            business.State = state1.Abbreviation;

            MockSqlQueryStorageContext<Business> mockBusinessStorage = new MockSqlQueryStorageContext<Business>(business);

            MockSqlQueryStorageContext<State> mockStateStorage = new MockSqlQueryStorageContext<State>(state1, state2);

            BusinessEditQuery query = new BusinessEditQuery()
            {
                BusinessID = business.ID
            };

            BusinessEditQueryHandler handler = new BusinessEditQueryHandler(mockBusinessStorage.Object, mockStateStorage.Object);
            BusinessEditModel model = await handler.ExecuteAsync(query);
            model.Should().NotBeNull();

            model.BusinessID.Should().Be(business.ID);
            model.Name.Should().Be(business.Name);
            model.Address.Should().Be(business.Address);
            model.City.Should().Be(business.City);
            model.State.Should().Be(business.State);
            model.Zip.Should().Be(business.Zip);

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