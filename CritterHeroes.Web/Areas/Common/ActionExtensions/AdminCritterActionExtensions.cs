using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Critters;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class AdminCritterActionExtensions
    {
        public static LinkElement AdminCrittersHomeActionLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink(nameof(CrittersController.Index), CrittersController.Route, AreaName.AdminRouteValue);
        }

        public static string AdminCrittersUploadJsonAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.UploadJson), CrittersController.Route, AreaName.AdminRouteValue);
        }

        public static string AdminCrittersUploadCsvAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.UploadCsv), CrittersController.Route, AreaName.AdminRouteValue);
        }

        public static string AdminCrittersUploadXmlAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.UploadXml), CrittersController.Route, AreaName.AdminRouteValue);
        }

        public static string AdminCrittersImportAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.Import), CrittersController.Route, AreaName.AdminRouteValue);
        }
    }
}
