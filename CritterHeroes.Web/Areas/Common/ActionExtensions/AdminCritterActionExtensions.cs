using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Critters;
using TOTD.Mvc.Actions;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class AdminCritterActionExtensions
    {
        public static LinkElement AdminCrittersHomeActionLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink(nameof(CrittersController.Index), ControllerRouteName, AreaName.AdminRouteValue);
        }

        public static string AdminCrittersUploadJsonAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.UploadJson), ControllerRouteName, AreaName.AdminRouteValue);
        }

        public static string AdminCrittersUploadCsvAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.UploadCsv), ControllerRouteName, AreaName.AdminRouteValue);
        }

        public static string AdminCrittersUploadXmlAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.UploadXml), ControllerRouteName, AreaName.AdminRouteValue);
        }

        public static string AdminCrittersImportAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.Import), ControllerRouteName, AreaName.AdminRouteValue);
        }

        private static string ControllerRouteName
        {
            get;
        } = ActionHelper.GetControllerRouteName(nameof(CrittersController));
    }
}
