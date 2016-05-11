using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Features.Admin.ErrorLog;

namespace CritterHeroes.Web.Features.Shared.ActionExtensions
{
    public static class ErrorLogActionExtensions
    {
        public static string ErrorLogHomeAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(ErrorLogController.Index), ErrorLogController.Route, AreaName.AdminRouteValue);
        }
    }
}
