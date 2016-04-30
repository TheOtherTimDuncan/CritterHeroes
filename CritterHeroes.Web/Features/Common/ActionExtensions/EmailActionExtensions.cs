using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Features.Admin.Emails;

namespace CritterHeroes.Web.Features.Common.ActionExtensions
{
    public static class EmailActionExtensions
    {
        public static string EmailHomeAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(EmailsController.Index), EmailsController.Route, AreaName.AdminRouteValue);
        }
    }
}
