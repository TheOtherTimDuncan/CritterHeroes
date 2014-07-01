using System;
using System.Collections.Generic;
using System.Web.Routing;
using AR.Website.Utility.FluentHtml.Contracts;
using TOTD.Utility.StringHelpers;

namespace AR.Website.Utility.FluentHtml
{
    public class InternalUrlContext : IUrlContext
    {
        public InternalUrlContext(string actionName, string controllerName, object routeValues)
            : this(actionName, controllerName)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary(routeValues);
            this.Area = routeValueDictionary["area"] as string;
        }

        public InternalUrlContext(string actionName, string controllerName)
            : this(actionName, controllerName, null)
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
    }
}