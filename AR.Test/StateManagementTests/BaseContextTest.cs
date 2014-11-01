using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AR.Domain.Contracts;
using Moq;

namespace AR.Test.StateManagementTests
{
    public class BaseContextTest
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
