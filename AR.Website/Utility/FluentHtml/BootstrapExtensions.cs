using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;

namespace AR.Website.Utility.FluentHtml
{
    public static class BootstrapExtensions
    {
        public static UnorderedListElement BootstrapNav(this HtmlHelper htmlHelper)
        {
            return new UnorderedListElement(htmlHelper.ViewContext).Class("nav").Class("navbar-nav");
        }

        public static ListItemElement AsBootstrapDropdown(this ListItemElement element)
        {
            return element.Class("dropdown");
        }

        public static UnorderedListElement BootstrapNavDropdown(this HtmlHelper htmlHelper)
        {
            return new UnorderedListElement(htmlHelper.ViewContext).Class("dropdown-menu");
        }

        public static ListItemElement BootstrapNavDropdownItem(this HtmlHelper htmlHelper, string text)
        {
            ListItemElement listItem = new ListItemElement(htmlHelper.ViewContext)
                .Class("dropdown")
                .AddElement
                (
                    new LinkElement(htmlHelper.ViewContext)
                        .Class("dropdown-toggle")
                        .Data("toggle", "dropdown")
                        .AsJavascriptLink()
                        .Text(text)
                        .AddElement
                        (
                            new SpanElement(htmlHelper.ViewContext)
                                .Class("caret")
                        )
                );
            return listItem;
        }
    }
}