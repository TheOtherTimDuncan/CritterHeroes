using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.People;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class AdminPeopleActionExtensions
    {
        public static LinkElement AdminPeopleHomeActionLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink(nameof(PeopleController.Index), PeopleController.Route, AreaName.AdminRouteValue);
        }

        public static string AdminPeopleImportAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(PeopleController.ImportPeople), PeopleController.Route, AreaName.AdminRouteValue);
        }

        public static string AdminBusinessImportAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(PeopleController.ImportBusinesses), PeopleController.Route, AreaName.AdminRouteValue);
        }
    }
}
