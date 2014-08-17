using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace AR.Website.Utility
{
    public static class HtmlHelperExtensions
    {
        public static void RenderAction(this HtmlHelper htmlHelper, string actionName, string controllerName, AreaName areaName)
        {
            htmlHelper.RenderAction(actionName, controllerName, routeValues: areaName.RouteValue);
        }
    }
}