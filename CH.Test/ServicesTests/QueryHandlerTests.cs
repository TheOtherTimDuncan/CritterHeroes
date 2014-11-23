using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Contracts.Queries;
using CH.Website.Models;
using CH.Website.Services.Queries;
using CH.Website.Services.QueryHandlers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.ServicesTests
{
    [TestClass]
    public class QueryHandlerTests : BaseTest
    {
        [TestMethod]
        public async Task CreatesViewModelFromQueryParameter()
        {
            LoginQuery query = new LoginQuery()
            {
                ReturnUrl="url"
            };
            LoginViewModelQueryHandler handler = new LoginViewModelQueryHandler();
            LoginModel model =await  handler.Retrieve(query);
            model.Should().NotBeNull();
            model.ReturnUrl.Should().Be(query.ReturnUrl);
        }
    }
}
