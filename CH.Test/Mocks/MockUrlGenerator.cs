using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts;
using Moq;

namespace CH.Test.Mocks
{
    public class MockUrlGenerator : Mock<IUrlGenerator>
    {
        public MockUrlGenerator()
           : this(new MockHttpContext())
        {
        }

        public MockUrlGenerator(MockHttpContext mockHttpContext)
        {
            this.BaseUrl = MockHttpContext.WebAppPath;

            this.UrlHelper = new MockUrlHelper(mockHttpContext);

            Setup(x => x.GenerateAbsoluteUrl(It.IsAny<string>(), It.IsAny<string>())).Returns((string actionName, string controllerName) =>
            {
                return this.UrlHelper.Action(actionName, controllerName, routeValues: null, protocol: this.UrlHelper.RequestContext.HttpContext.Request.Url.Scheme);
            });

            Setup(x => x.GenerateAbsoluteUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>())).Returns((string actionName, string controllerName, object routeValues) =>
            {
                return this.UrlHelper.Action(actionName, controllerName, routeValues, protocol: this.UrlHelper.RequestContext.HttpContext.Request.Url.Scheme);
            });
        }

        public MockUrlHelper UrlHelper
        {
            get;
            private set;
        }

        public string BaseUrl
        {
            get;
            private set;
        }
    }
}
