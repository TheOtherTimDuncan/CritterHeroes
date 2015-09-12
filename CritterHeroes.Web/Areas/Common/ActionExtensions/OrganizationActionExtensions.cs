using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Organizations;
using TOTD.Mvc.Actions;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class OrganizationActionExtensions
    {
        public static LinkElement OrganizationEditProfileActionLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink(nameof(OrganizationController.EditProfile), ControllerRouteName, AreaName.AdminRouteValue);
        }

        private static string ControllerRouteName
        {
            get;
        } = ActionHelper.GetControllerRouteName(nameof(OrganizationController));
    }
}
