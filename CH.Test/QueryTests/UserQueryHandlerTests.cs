using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts.Identity;
using CH.Domain.Identity;
using CH.Domain.Queries;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.QueryTests
{
    [TestClass]
    public class UserQueryHandlerTests : BaseTest
    {
        [TestMethod]
        public async Task ReturnsUser()
        {
            UserQuery query = new UserQuery()
            {
                Username="unit.test"
            };

            IdentityUser user = new IdentityUser(query.Username);

            Mock<IApplicationUserStore> mockUserStore = new Mock<IApplicationUserStore>();
            mockUserStore.Setup(x => x.FindByNameAsync(query.Username)).Returns(Task.FromResult(user));

            UserQueryHandler handler = new UserQueryHandler(mockUserStore.Object);
            (await handler.Retrieve(query)).Should().Equals(user);

            mockUserStore.Verify(x => x.FindByNameAsync(query.Username), Times.Once);
        }
    }
}
