using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.Proxies;
using FluentAssertions;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test
{
    [TestClass]
    public class ProxyTests
    {
        [TestMethod]
        public void HttpUserProxyReturnsCorrectProperties()
        {
            GenericIdentity genericIdentity = new GenericIdentity("user.name");
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(genericIdentity);
            claimsIdentity.AddClaim(new Claim(AppClaimTypes.UserID, "userid"));

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request.User).Returns(new ClaimsPrincipal(claimsIdentity));

            HttpUserProxy userProxy = new HttpUserProxy(mockOwinContext.Object);

            userProxy.Username.Should().Be(genericIdentity.Name);
            userProxy.UserID.Should().Be("userid");
        }
    }
}
