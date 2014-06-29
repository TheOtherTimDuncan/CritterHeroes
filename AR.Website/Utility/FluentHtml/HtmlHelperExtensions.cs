using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;

namespace AR.Website.Utility.FluentHtml
{
    public static class HtmlHelperExtensions
    {
        public static LinkElement Link(this HtmlHelper htmlHelper)
        {
            return new LinkElement(htmlHelper.ViewContext);
        }

        public static ListItemElement ListItem(this HtmlHelper htmlHelper)
        {
            return new ListItemElement(htmlHelper.ViewContext);
        }

        public static UnorderedListElement UnorderedList(this HtmlHelper htmlHelper)
        {
            return new UnorderedListElement(htmlHelper.ViewContext);
        }

        public static SpanElement Span(this HtmlHelper htmlHelper)
        {
            return new SpanElement(htmlHelper.ViewContext);
        }
    }
}