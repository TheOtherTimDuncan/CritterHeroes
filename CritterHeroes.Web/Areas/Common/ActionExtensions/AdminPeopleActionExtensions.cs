using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.People;
using TOTD.Mvc.Actions;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class AdminPeopleActionExtensions
    {
        public static LinkElement AdminPeopleHomeActionLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink(nameof(PeopleController.Index), ControllerRouteName, AreaName.AdminRouteValue);
        }

        public static string AdminPeopleImportAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(PeopleController.ImportPeople), ControllerRouteName, AreaName.AdminRouteValue);
        }

        public static string AdminBusinessImportAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(PeopleController.ImportBusinesses), ControllerRouteName, AreaName.AdminRouteValue);
        }

        private static string ControllerRouteName
        {
            get;
        } = ActionHelper.GetControllerRouteName(nameof(PeopleController));
    }
}
