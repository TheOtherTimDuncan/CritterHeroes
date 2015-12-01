using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Organizations;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class OrganizationActionExtensions
    {
        public static string OrganizationEditProfileAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(OrganizationController.EditProfile), OrganizationController.Route, AreaName.AdminRouteValue);
        }
    }
}
