using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Lists;
using CritterHeroes.Web.Areas.Common.Models;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class ListsActionExtensions
    {
        public static ControllerActionModel HomeAction(string title)
        {
            return new ControllerActionModel()
            {
                ControllerRoute = ListsController.Route,
                ActionName = nameof(ListsController.Index),
                AreaName = AreaName.Admin,
                Title = title
            };
        }

        public static string ListsRefreshAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(ListsController.Refresh), ListsController.Route, AreaName.AdminRouteValue);
        }

        public static string ListsSyncAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(ListsController.Sync), ListsController.Route, AreaName.AdminRouteValue);
        }
    }
}
