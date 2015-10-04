using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CritterHeroes.Web.Areas.Common.Models;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class PagingExtensions
    {
        public static MvcHtmlString Paging(this HtmlHelper htmlHelper, PagingModel pagingModel)
        {
            return htmlHelper.Partial("_Paging", pagingModel);
        }
    }
}
