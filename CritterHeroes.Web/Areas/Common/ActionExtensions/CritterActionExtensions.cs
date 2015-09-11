using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CritterHeroes.Web.Areas.Critters;
using CritterHeroes.Web.Contracts;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class CritterActionExtensions
    {
        public static void RenderCritterHomeAction(this HtmlHelper htmlHelper)
        {
            string controllerName = nameof(CrittersController);
            controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);
            htmlHelper.RenderAction(nameof(CrittersController.Index), controllerName);
        }

        public static string GenerateAbsoluteHomeUrl(this IUrlGenerator urlGenerator)
        {
            return urlGenerator.GenerateAbsoluteUrl<CrittersController>(x => x.Index());
        }

        public static string HomeAction(this UrlHelper urlHelper)
        {
            string controllerName = nameof(CrittersController);
            controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);
            return urlHelper.Action(nameof(CrittersController.Index), controllerName);
        }

        public static LinkElement CritterHomeActionLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink<CrittersController>(x => x.Index());
        }

        public static string Local(this UrlHelper urlHelper, string url)
        {
            if (urlHelper.IsLocalUrl(url))
            {
                return url;
            };
            return urlHelper.HomeAction();
        }

        private static string ControllerName
        {
            get;
        } = GetControllerRouteName();

        private static string GetControllerRouteName()
        {
            string controllerName = nameof(CrittersController);
            controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);
            return controllerName;
        }
    }
}
