using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using FluentAssertions;
using TOTD.Mvc.FluentHtml;

namespace CH.Test.ControllerTests
{
    public class BaseControllerTest : BaseTest
    {
        public RouteCollection GetRouteCollection()
        {
            RouteCollection result = new RouteCollection();
            result.Add(new Route("{controller}/{action}/{id}", null)
            {
                Defaults = new RouteValueDictionary(new
                {
                    id = "defaultid"
                })
            });
            return result;
        }

        public void VerifyRedirectToRouteResult(RedirectToRouteResult redirectResult, string actionName, string controllerName)
        {
            redirectResult.Should().NotBeNull();
            redirectResult.RouteValues[RouteValueKeys.Controller].Should().Be(controllerName);
            redirectResult.RouteValues[RouteValueKeys.Action].Should().Be(actionName);
        }
    }
}
