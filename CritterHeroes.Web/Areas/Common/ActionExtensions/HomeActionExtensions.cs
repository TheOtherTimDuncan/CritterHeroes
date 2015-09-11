using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Home;
using TOTD.Mvc.Actions;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class HomeActionExtensions
    {
        public static LinkElement HomeActionLink(this LinkElement linkElement, Expression<Func<HomeController, ActionResult>> actionSelector)
        {
            return linkElement.ActionLink<HomeController>(actionSelector);
        }

        public static void RenderHomeAction(this HtmlHelper htmlHelper, Expression<Func<HomeController, ActionResult>> actionSelector)
        {
            htmlHelper.RenderAction(actionSelector, null);
        }

        public static string HomeAction(this UrlHelper urlHelper, Expression<Func<HomeController, ActionResult>> actionSelector)
        {
            return urlHelper.Action<HomeController>(actionSelector);
        }
    }
}