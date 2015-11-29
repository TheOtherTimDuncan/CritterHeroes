using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.People;
using CritterHeroes.Web.Areas.Common.Models;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class AdminPeopleActionExtensions
    {
        public static ControllerActionModel HomeAction(string title)
        {
            return new ControllerActionModel()
            {
                ControllerRoute = PeopleController.Route,
                ActionName = nameof(PeopleController.Index),
                AreaName = AreaName.Admin,
                Title = title
            };
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
