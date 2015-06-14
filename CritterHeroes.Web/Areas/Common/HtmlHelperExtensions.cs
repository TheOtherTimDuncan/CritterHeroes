using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CritterHeroes.Web.Areas.Common.ActionExtensions;
using CritterHeroes.Web.Areas.Home;
using CritterHeroes.Web.Areas.Models.Modal;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Middleware;
using TOTD.Mvc.Actions;
using TOTD.Utility.Misc;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Areas.Common
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString ModalDialog(this HtmlHelper htmlHelper, ModalDialogModel model)
        {
            if (model == null)
            {
                return null;
            }
            return htmlHelper.Partial("_ModalDialog", model);
        }

        public static MvcHtmlString CancelButton(this HtmlHelper htmlHelper)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.AddCssClass("btn btn-info");
            builder.SetInnerText("Cancel");
            builder.MergeAttribute("title", "Cancel");

            IPageContextService pageContextService = DependencyResolver.Current.GetService<IPageContextService>();
            PageContext pageContext = pageContextService.GetPageContext();

            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            string previousUrl = urlHelper.Local(pageContext.IfNotNull(x => x.PreviousPath));
            builder.MergeAttribute("href", previousUrl);

            return new MvcHtmlString(builder.ToString());
        }
    }
}