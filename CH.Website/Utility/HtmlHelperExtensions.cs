using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CH.Website.Controllers;
using TOTD.Mvc.Actions;

namespace CH.Website.Utility
{
    public static class HtmlHelperExtensions
    {
        public static void RenderHomeAction(this HtmlHelper htmlHelper, Expression<Func<HomeController, ActionResult>> actionSelector)
        {
            htmlHelper.RenderAction(actionSelector, null);
        }
    }
}