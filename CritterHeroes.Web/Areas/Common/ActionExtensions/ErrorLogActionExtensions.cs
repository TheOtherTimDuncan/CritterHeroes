using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.ErrorLog;
using CritterHeroes.Web.Areas.Common.Models;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class ErrorLogActionExtensions
    {
        public static ControllerActionModel HomeAction(string title)
        {
            return new ControllerActionModel()
            {
                ControllerRoute = ErrorLogController.Route,
                ActionName = nameof(ErrorLogController.Index),
                AreaName = AreaName.Admin,
                Title = title
            };
        }
    }
}
