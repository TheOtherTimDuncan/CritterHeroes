using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Features.Admin.Contacts.Models;
using CritterHeroes.Web.Features.Admin.Contacts.Queries;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TOTD.Utility.UnitTestHelpers;

namespace CH.Test.AdminContactsTests
{
    [TestClass]
    public class ContactsQueryHandlerTests
    {
        [TestMethod]
        public async Task ReturnsViewModel()
        {
            Group group1 = new Group("group1")
            {
                IsBusiness = true,
                IsPerson = false
            }.SetEntityID(x => x.ID);

            Group group2 = new Group("group2")
            {
                IsBusiness = false,
                IsPerson = true
            }.SetEntityID(x => x.ID);

            ContactsQuery query = new ContactsQuery()
            {
                GroupID = group2.ID
            };

            MockSqlStorageContext<Group> mockGroupStorage = new MockSqlStorageContext<Group>(group1, group2);

            ContactsQueryHandler handler = new ContactsQueryHandler(mockGroupStorage.Object);
            ContactsModel model = await handler.ExecuteAsync(query);

            model.Query.Should().Be(query);

            model.GroupItems.Should().HaveCount(2);

            GroupSelectOptionModel groupModel1 = model.GroupItems.SingleOrDefault(x => x.Value == group1.ID);
            groupModel1.Should().NotBeNull();
            groupModel1.Text.Should().Be(group1.Name);
            groupModel1.IsBusiness.Should().Be(group1.IsBusiness);
            groupModel1.IsPerson.Should().Be(group1.IsPerson);
            groupModel1.IsSelected.Should().BeFalse();

            GroupSelectOptionModel groupModel2 = model.GroupItems.SingleOrDefault(x => x.Value == group2.ID);
            groupModel2.Should().NotBeNull();
            groupModel2.Text.Should().Be(group2.Name);
            groupModel2.IsBusiness.Should().Be(group2.IsBusiness);
            groupModel2.IsPerson.Should().Be(group2.IsPerson);
            groupModel2.IsSelected.Should().BeTrue();
        }
    }
}
