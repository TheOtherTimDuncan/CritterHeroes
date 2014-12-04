using System;
using System.Collections.Generic;
using System.Linq;
using CH.Domain.Contracts;
using CH.Domain.Identity;
using CH.Domain.StateManagement;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.StateManagementTests
{
    [TestClass]
    public class UserContextTests : BaseContextTest
    {
        [TestMethod]
        public void CanGetAndSaveUserContext()
        {
            UserContext context = new UserContext()
            {
                DisplayName = "display name"
            };

            UserStateManager stateManager = new UserStateManager(GetMockHttpContext().Object);
            stateManager.SaveContext(context);
            UserContext result = stateManager.GetContext();
            result.DisplayName.Should().Be(context.DisplayName);
        }

        [TestMethod]
        public void CanCreateItselfFromUser()
        {
            IdentityUser user = new IdentityUser("unit.test")
            {
                FirstName = "first",
                LastName = "last"
            };

            UserContext userContext = UserContext.FromUser(user);
            userContext.DisplayName.Should().Be("first last");
        }

        [TestMethod]
        public void StateManagerReturnsNullIfDisplayNameIsMissing()
        {
            UserContext context = new UserContext()
            {
                DisplayName = null
            };

            Mock<IHttpContext> mockHttpContext = GetMockHttpContext();
            mockHttpContext.Object.Request.Cookies.Should().HaveCount(0);

            UserStateManager stateManager = new UserStateManager(mockHttpContext.Object);
            stateManager.SaveContext(context);
            mockHttpContext.Object.Request.Cookies.Should().HaveCount(1);
            stateManager.GetContext().Should().BeNull();
        }
    }
}
