using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CritterHeroes.Web.Areas.Home;
using TOTD.Mvc.Actions;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class HomeActionExtensions
    {
        public static void RenderHomeMenuAction(this HtmlHelper htmlHelper)
        {
            htmlHelper.RenderAction(nameof(HomeController.Menu), ControllerRouteName);
        }

        public static void RenderHomeHeaderAction(this HtmlHelper htmlHelper)
        {
            htmlHelper.RenderAction(nameof(HomeController.Header), ControllerRouteName);
        }

        private static string ControllerRouteName
        {
            get;
        } = ActionHelper.GetControllerRouteName(nameof(HomeController));
    }
}
