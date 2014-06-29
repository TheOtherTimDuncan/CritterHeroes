using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AR.Website.Utility.FluentHtml.Elements;

namespace AR.Website.Utility
{
    public static class ElementExtensions
    {
        public static LinkElement ActionLink(this LinkElement linkElement, string actionName, string controllerName, AreaName areaName)
        {
            return linkElement.ActionLink(actionName, controllerName, areaName.RouteValue);
        }
    }
}