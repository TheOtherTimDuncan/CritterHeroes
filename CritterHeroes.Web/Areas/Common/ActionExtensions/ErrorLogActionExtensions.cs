using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.ErrorLog;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class ErrorLogActionExtensions
    {
        public static LinkElement ErrorLogHomeActionLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink(nameof(ErrorLogController.Index), ErrorLogController.Route, AreaName.AdminRouteValue);
        }
    }
}
