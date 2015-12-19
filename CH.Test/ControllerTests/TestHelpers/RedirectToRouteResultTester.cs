using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using FluentAssertions;
using TOTD.Mvc.FluentHtml;

namespace CH.Test.ControllerTests.TestHelpers
{
    public class RedirectToRouteResultTester
    {
        private RedirectToRouteResult _redirectResult;

        public RedirectToRouteResultTester(RedirectToRouteResult redirectResult)
        {
            this._redirectResult = redirectResult;
        }

        public RedirectToRouteResultTester HavingControllerRoute(string route)
        {
            _redirectResult.RouteValues[RouteValueKeys.Controller].Should().Be(route);
            return this;
        }

        public RedirectToRouteResultTester HavingActionRoute(string actionName)
        {
            _redirectResult.RouteValues[RouteValueKeys.Action].Should().Be(actionName);
            return this;
        }

        public RedirectToRouteResultTester HavingRouteValues(object routeValues)
        {
            RouteValueDictionary testValues = new RouteValueDictionary(routeValues);
            foreach (var keyValue in testValues)
            {
                _redirectResult.RouteValues.Should().Contain(keyValue);
            }
            return this;
        }
    }
}
