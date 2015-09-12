using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace CH.Test.Mocks
{
    public class MockUrlHelper : UrlHelper
    {
        public MockUrlHelper(MockHttpContext mockHttpContext)
            : base(mockHttpContext.Object.Request.RequestContext, new RouteCollection())
        {
            this.RouteCollection.Add(new Route("{controller}/{action}/{id}", null)
            {
                Defaults = new RouteValueDictionary(new
                {
                    id = "defaultid"
                })
            });
        }

        public string AbsoluteAction(string actionName, string controllerName)
        {
            return AbsoluteAction(actionName, controllerName, null);
        }

        public string AbsoluteAction(string actionName, string controllerName, object routeValues)
        {
            return Action(actionName, controllerName, routeValues, RequestContext.HttpContext.Request.Url.Scheme);
        }
    }
}
