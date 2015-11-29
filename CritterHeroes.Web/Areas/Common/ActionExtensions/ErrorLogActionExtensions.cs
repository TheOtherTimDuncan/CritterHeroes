using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.ErrorLog;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class ErrorLogActionExtensions
    {
        public static string ErrorLogHomeAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(ErrorLogController.Index), ErrorLogController.Route, AreaName.AdminRouteValue);
        }
    }
}
