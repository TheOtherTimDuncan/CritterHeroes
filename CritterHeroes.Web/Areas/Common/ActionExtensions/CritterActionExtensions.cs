using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using CritterHeroes.Web.Areas.Critters;
using CritterHeroes.Web.Areas.Critters.Queries;
using CritterHeroes.Web.Contracts;
using TOTD.Mvc.FluentHtml;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class CritterActionExtensions
    {
        public static void RenderCritterHomeAction(this HtmlHelper htmlHelper)
        {
            htmlHelper.RenderAction(nameof(CrittersController.Index), CrittersController.Route);
        }

        public static string GenerateAbsoluteHomeUrl(this IUrlGenerator urlGenerator)
        {
            return urlGenerator.GenerateAbsoluteUrl(nameof(CrittersController.Index), CrittersController.Route);
        }

        public static string HomeAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.Index), CrittersController.Route);
        }

        public static string CritterHomeAction(this UrlHelper urlHelper, int? statusID = null)
        {
            CrittersQuery query = new CrittersQuery()
            {
                StatusID = statusID
            };

            RouteValueDictionary routeValues = new RouteValueDictionary(query);
            routeValues[RouteValueKeys.Area] = "";

            return urlHelper.Action(nameof(CrittersController.Index), CrittersController.Route, routeValues);
        }

        public static string CritterListAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.List), CrittersController.Route);
        }

        public static string Local(this UrlHelper urlHelper, string url)
        {
            if (urlHelper.IsLocalUrl(url))
            {
                return url;
            };
            return urlHelper.HomeAction();
        }
    }
}
