using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Common.ActionExtensions;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using TOTD.Utility.Misc;

namespace CritterHeroes.Web.Areas.Common
{
    public static class HtmlHelperExtensions
    {
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