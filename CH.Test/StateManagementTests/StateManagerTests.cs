using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CH.Domain.Contracts;
using CH.Domain.StateManagement;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.StateManagementTests
{
    [TestClass]
    public class StateManagerTests : BaseContextTest
    {
        [TestMethod]
        public void ThrowsExceptionForNullHttpContext()
        {
            Action action = () => new FakeStateManager(null, "test");
            action
                .ShouldThrow<ArgumentException>()
                .And
                .ParamName.Should().Be("httpContext");
        }

        [TestMethod]
        public void ThrowsExceptionForNullKey()
        {
            Mock<IHttpContext> mockHttpContext = new Mock<IHttpContext>();
            Action action = () => new FakeStateManager(mockHttpContext.Object, null);
            action
                .ShouldThrow<ArgumentException>()
                .And
                .ParamName.Should().Be("key");
        }

        [TestMethod]
        public void ThrowsExceptionForEmptyKey()
        {
            Mock<IHttpContext> mockHttpContext = new Mock<IHttpContext>();
            Action action = () => new FakeStateManager(mockHttpContext.Object, string.Empty);
            action
                .ShouldThrow<ArgumentException>()
                .And
                .ParamName.Should().Be("key");
        }

        [TestMethod]
        public void CombinesGivenKeyWithBaseKey()
        {
            Mock<IHttpContext> mockHttpContext = GetMockHttpContext();
            FakeStateManager stateManager = new FakeStateManager(mockHttpContext.Object, "key");
            stateManager.SaveContext("test");

            mockHttpContext.Object.Response.Cookies["CritterHeroes.key"].Should().NotBeNull();
        }

        [TestMethod]
        public void CreatesSecureCookie()
        {
            Mock<IHttpContext> mockHttpContext = GetMockHttpContext();
            FakeStateManager stateManager = new FakeStateManager(mockHttpContext.Object, "key");
            stateManager.SaveContext("test");

            HttpCookie cookie = mockHttpContext.Object.Response.Cookies["CritterHeroes.key"];
            cookie.Should().NotBeNull();
            cookie.HttpOnly.Should().Be(true);
            cookie.Secure.Should().Be(true);
        }
    }

    public class FakeStateManager : StateManager<string>
    {
        public FakeStateManager(IHttpContext httpContext, string key)
            : base(httpContext, key)
        {
        }
    }
}
