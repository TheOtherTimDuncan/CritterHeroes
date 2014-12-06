using System;
using System.Collections.Generic;
using System.Linq;
using CH.Domain.Identity;
using CH.Domain.StateManagement;
using FluentAssertions;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace CH.Test.StateManagementTests
{
    [TestClass]
    public class UserContextTests : BaseContextTest
    {
        [TestMethod]
        public void CanGetUserContext()
        {
            UserContext context = new UserContext()
            {
                DisplayName = "display name"
            };

            Dictionary<string, string> cookies = new Dictionary<string, string>();
            cookies["CritterHeroes.User"] = JsonConvert.SerializeObject(context);

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request.Cookies).Returns(new RequestCookieCollection(cookies));

            UserStateManager stateManager = new UserStateManager(mockOwinContext.Object);
            UserContext result = stateManager.GetContext();
            result.DisplayName.Should().Be(context.DisplayName);

            mockOwinContext.Verify(x => x.Request.Cookies, Times.Once);
        }

        [TestMethod]
        public void CanSaveUserContext()
        {
            UserContext context = new UserContext()
            {
                DisplayName = "display name"
            };

            Dictionary<string, string[]> cookies = new Dictionary<string, string[]>();

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Response.Cookies).Returns(new ResponseCookieCollection(new HeaderDictionary(cookies)));

            UserStateManager stateManager = new UserStateManager(mockOwinContext.Object);
            stateManager.SaveContext(context);

            cookies.Should().HaveCount(1);
            cookies.First().Value[0].Should().Contain("CritterHeroes.User");

            mockOwinContext.Verify(x => x.Response.Cookies, Times.Once);
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

            Dictionary<string, string> cookies = new Dictionary<string, string>();
            cookies["CritterHeroes.User"] = JsonConvert.SerializeObject(context);

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request.Cookies).Returns(new RequestCookieCollection(cookies));

            UserStateManager stateManager = new UserStateManager(mockOwinContext.Object);
            stateManager.GetContext().Should().BeNull();

            mockOwinContext.Verify(x => x.Request.Cookies, Times.Once);
        }
    }
}
