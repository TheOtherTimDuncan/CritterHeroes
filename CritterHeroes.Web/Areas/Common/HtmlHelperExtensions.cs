using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CritterHeroes.Web.Areas.Home;
using CritterHeroes.Web.Areas.Models.Modal;
using TOTD.Mvc.Actions;

namespace CritterHeroes.Web.Areas.Common
{
    public static class HtmlHelperExtensions
    {
        public static void RenderHomeAction(this HtmlHelper htmlHelper, Expression<Func<HomeController, ActionResult>> actionSelector)
        {
            htmlHelper.RenderAction(actionSelector, null);
        }

        public static MvcHtmlString ModalDialog(this HtmlHelper htmlHelper, ModalDialogModel model)
        {
            if (model == null)
            {
                return null;
            }
            return htmlHelper.Partial("_ModalDialog", model);
        }
    }
}