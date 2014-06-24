using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;

namespace AR.Website.Utility.FluentHtml
{
    public static class HtmlHelperExtensions
    {
        public static LinkElement Link(this HtmlHelper htmlHelper)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            return new LinkElement(urlHelper);
        }
    }
}