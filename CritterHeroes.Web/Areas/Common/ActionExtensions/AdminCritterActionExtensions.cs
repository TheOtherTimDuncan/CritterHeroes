using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using CritterHeroes.Web.Areas.Admin.Critters;
using CritterHeroes.Web.Areas.Admin.Critters.Queries;
using CritterHeroes.Web.Contracts;
using TOTD.Mvc.FluentHtml;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class AdminCritterActionExtensions
    {
        public static void RenderCritterHomeAction(this HtmlHelper htmlHelper)
        {
            htmlHelper.RenderAction(nameof(CrittersController.Index), CrittersController.Route, AreaName.AdminRouteValue);
        }

        public static string GenerateAbsoluteHomeUrl(this IUrlGenerator urlGenerator)
        {
            return urlGenerator.GenerateAbsoluteUrl(nameof(CrittersController.Index), CrittersController.Route, AreaName.AdminRouteValue);
        }

        public static string HomeAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.Index), CrittersController.Route, AreaName.AdminRouteValue);
        }

        public static string CritterHomeAction(this UrlHelper urlHelper, int? statusID = null)
        {
            CrittersQuery query = new CrittersQuery()
            {
                StatusID = statusID
            };

            RouteValueDictionary routeValues = new RouteValueDictionary(query);
            routeValues[RouteValueKeys.Area] = AreaName.Admin;

            return urlHelper.Action(nameof(CrittersController.Index), CrittersController.Route, routeValues);
        }

        public static string CritterListAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.List), CrittersController.Route, AreaName.AdminRouteValue);
        }

        public static string Local(this UrlHelper urlHelper, string url)
        {
            if (urlHelper.IsLocalUrl(url))
            {
                return url;
            };
            return urlHelper.HomeAction();
        }

        public static string AdminCrittersHomeAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.Summary), CrittersController.Route, AreaName.AdminRouteValue);
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
