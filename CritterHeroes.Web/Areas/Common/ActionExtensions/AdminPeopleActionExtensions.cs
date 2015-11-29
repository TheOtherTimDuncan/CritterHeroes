using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.People;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class AdminPeopleActionExtensions
    {
        public static string AdminPeopleHomeAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(PeopleController.Index), PeopleController.Route, AreaName.AdminRouteValue);
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
