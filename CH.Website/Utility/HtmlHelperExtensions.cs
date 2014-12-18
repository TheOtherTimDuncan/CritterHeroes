using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CH.Website.Controllers;
using CH.Website.Models.Modal;
using TOTD.Mvc.Actions;

namespace CH.Website.Utility
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