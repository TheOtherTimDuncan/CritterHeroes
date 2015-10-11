using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CritterHeroes.Web.Areas.Critters;
using CritterHeroes.Web.Contracts;
using TOTD.Mvc.Actions;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class CritterActionExtensions
    {
        public static void RenderCritterHomeAction(this HtmlHelper htmlHelper)
        {
            htmlHelper.RenderAction(nameof(CrittersController.Index), ControllerRouteName);
        }

        public static string GenerateAbsoluteHomeUrl(this IUrlGenerator urlGenerator)
        {
            return urlGenerator.GenerateAbsoluteUrl(nameof(CrittersController.Index), ControllerRouteName);
        }

        public static string HomeAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.Index), ControllerRouteName);
        }

        public static string CritterHomeAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.Index), ControllerRouteName);
        }

        public static string CritterListAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(CrittersController.List), ControllerRouteName);
        }

        public static LinkElement CritterHomeActionLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink(nameof(CrittersController.Index), ControllerRouteName);
        }

        public static string Local(this UrlHelper urlHelper, string url)
        {
            if (urlHelper.IsLocalUrl(url))
            {
                return url;
            };
            return urlHelper.HomeAction();
        }

        public static string ControllerRouteName
        {
            get;
        } = ActionHelper.GetControllerRouteName(nameof(CrittersController));
    }
}
