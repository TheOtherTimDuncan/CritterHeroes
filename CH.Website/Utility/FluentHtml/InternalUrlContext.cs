using System;
using System.Collections.Generic;
using System.Web.Routing;
using CH.Website.Utility.FluentHtml.Contracts;
using TOTD.Utility.StringHelpers;

namespace CH.Website.Utility.FluentHtml
{
    public class InternalUrlContext : IUrlContext
    {
        public InternalUrlContext(string actionName, string controllerName, RouteValueDictionary routeValues)
            : this(actionName, controllerName, GetAreaFromRouteValues(routeValues))
        {
        }

        public InternalUrlContext(string actionName, string controllerName, object routeValues)
            : this(actionName, controllerName, GetAreaFromRouteValues(new RouteValueDictionary(routeValues)))
        {
        }

        public InternalUrlContext(string actionName, string controllerName)
            : this(actionName, controllerName, (string)null)
        {
        }

        public InternalUrlContext(string actionName, string controllerName, string areaName)
        {
            this.ActionName = actionName;
            this.ControllerName = controllerName;
            this.Area = areaName;
        }

        public string ControllerName
        {
            get;
            private set;
        }

        public string ActionName
        {
            get;
            private set;
        }

        public string Area
        {
            get;
            private set;
        }

        public bool Matches(IUrlContext urlContext)
        {
            InternalUrlContext otherUlr = urlContext as InternalUrlContext;

            if (otherUlr == null)
            {
                return false;
            }

            return this.Area.SafeEquals(otherUlr.Area) && this.ControllerName.SafeEquals(otherUlr.ControllerName) && this.ActionName.SafeEquals(otherUlr.ActionName);
        }

        private static string GetAreaFromRouteValues(RouteValueDictionary routeValues)
        {
            return routeValues[RouteDataKeys.Area] as string;
        }
    }
}