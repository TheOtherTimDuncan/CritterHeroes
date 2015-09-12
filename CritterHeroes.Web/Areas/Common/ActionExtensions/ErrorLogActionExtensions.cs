using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.ErrorLog;
using TOTD.Mvc.Actions;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class ErrorLogActionExtensions
    {
        public static LinkElement ErrorLogHomeActionLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink(nameof(ErrorLogController.Index), ControllerRouteName, AreaName.AdminRouteValue);
        }

        private static string ControllerRouteName
        {
            get;
        } = ActionHelper.GetControllerRouteName(nameof(ErrorLogController));
    }
}
