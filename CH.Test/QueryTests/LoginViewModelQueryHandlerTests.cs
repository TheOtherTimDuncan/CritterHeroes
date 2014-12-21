using System;
using System.Collections.Generic;
using System.Linq;
using CH.Website.Models.Account;
using CH.Website.Services.Queries;
using CH.Website.Services.QueryHandlers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.QueryHandlerTests
{
    [TestClass]
    public class LoginViewModelQueryHandlerTests : BaseTest
    {
        [TestMethod]
        public void LoginQueryHandlerReturnsViewModel()
        {
            LoginQuery query = new LoginQuery()
            {
                ReturnUrl = "url"
            };
            LoginViewModelQueryHandler handler = new LoginViewModelQueryHandler();
            LoginModel model = handler.Retrieve(query);
            model.Should().NotBeNull();
            model.ReturnUrl.Should().Be(query.ReturnUrl);
        }
    }
}
