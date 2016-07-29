using System;
using System.Collections.Generic;
using System.Linq;
using CH.Test.ControllerTests.TestHelpers;
using CritterHeroes.Web.Features.Admin.Contacts;
using CritterHeroes.Web.Features.Admin.Contacts.Models;
using CritterHeroes.Web.Features.Admin.Contacts.Queries;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.ControllerTests
{
    [TestClass]
    public class ContactsControllerTest : BaseTest
    {
        [TestMethod]
        public void IndexGetReturnsViewWithModel()
        {
            ContactsModel model = new ContactsModel();
            ContactsQuery query = new ContactsQuery();

            ControllerTester.UsingController<ContactsController>()
                .SetupQueryDispatcherAsync(model, query)
                .WithCallTo(x => x.Index(query))
                .VerifyQueryDispatcher()
                .ShouldReturnViewResult()
                .HavingDefaultView()
                .HavingModel(model);
        }

        [TestMethod]
        public void ListGetReturnsJson()
        {
            ContactsListModel model = new ContactsListModel();
            ContactsListQuery query = new ContactsListQuery();

            ControllerTester.UsingController<ContactsController>()
                .SetupQueryDispatcherAsync(model, query)
                .WithCallTo(x => x.List(query))
                .VerifyQueryDispatcher()
                .ShouldReturnJsonCamelCase()
                .HavingModel(model);
        }

        [TestMethod]
        public void PersonGetReturnsViewWithModel()
        {
            PersonEditModel model = new PersonEditModel();
            PersonEditQuery query = new PersonEditQuery();

            ControllerTester.UsingController<ContactsController>()
                .SetupQueryDispatcherAsync(model, query)
                .WithCallTo(x => x.Person(query))
                .VerifyQueryDispatcher()
                .ShouldReturnViewResult()
                .HavingDefaultView()
                .HavingModel(model);
        }

        [TestMethod]
        public void BusinessGetReturnsViewWithModel()
        {
            BusinessEditModel model = new BusinessEditModel();
            BusinessEditQuery query = new BusinessEditQuery();

            ControllerTester.UsingController<ContactsController>()
                .SetupQueryDispatcherAsync(model, query)
                .WithCallTo(x => x.Business(query))
                .VerifyQueryDispatcher()
                .ShouldReturnViewResult()
                .HavingDefaultView()
                .HavingModel(model);
        }
    }
}
