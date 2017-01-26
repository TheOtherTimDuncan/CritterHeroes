using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CritterHeroes.Web.Domain.Contracts;
using Moq;

namespace CH.Test.StateManagementTests
{
    public class BaseContextTest : BaseTest
    {
        public Mock<IHttpContext> GetMockHttpContext()
        {
            HttpCookieCollection cookies = new HttpCookieCollection();
            Mock<IHttpContext> mockHttpContext = new Mock<IHttpContext>();
            mockHttpContext.Setup(x => x.Response.Cookies).Returns(cookies);
            mockHttpContext.Setup(x => x.Request.Cookies).Returns(cookies);
            return mockHttpContext;
        }
    }
}
