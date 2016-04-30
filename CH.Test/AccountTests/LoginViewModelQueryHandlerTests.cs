using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Features.Account.Models;
using CritterHeroes.Web.Features.Account.Queries;
using CritterHeroes.Web.Features.Account.QueryHandlers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.AccountTests
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
            LoginModel model = handler.Execute(query);
            model.Should().NotBeNull();
            model.ReturnUrl.Should().Be(query.ReturnUrl);
        }
    }
}
