using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Critters;
using CritterHeroes.Web.Areas.Common.Models;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class AdminCritterActionExtensions
    {
        public static ControllerActionModel HomeAction(string title)
        {
            return new ControllerActionModel()
            {
                ControllerRoute = CrittersController.Route,
                ActionName = nameof(CrittersController.Index),
                AreaName = AreaName.Admin,
                Title = title
            };
        }

        public static string AdminCrittersHomeAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.Index), CrittersController.Route, AreaName.AdminRouteValue);
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
