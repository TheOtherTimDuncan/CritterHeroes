using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Lists;
using TOTD.Mvc.Actions;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class ListsActionExtensions
    {
        public static LinkElement ListsHomeActionLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink(nameof(ListsController.Index), ControllerRouteName, AreaName.AdminRouteValue);
        }

        public static string ListsRefreshAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(ListsController.Refresh), ControllerRouteName, AreaName.AdminRouteValue);
        }

        public static string ListsSyncAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(ListsController.Sync), ControllerRouteName, AreaName.AdminRouteValue);
        }

        private static string ControllerRouteName
        {
            get;
        } = ActionHelper.GetControllerRouteName(nameof(ListsController));
    }
}
