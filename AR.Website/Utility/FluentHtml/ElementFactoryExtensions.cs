using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;

namespace AR.Website.Utility.FluentHtml
{
    public static class ElementFactoryExtensions
    {
        public static LinkElement Link(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<LinkElement>();
        }

        public static ListItemElement ListItem(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<ListItemElement>();
        }

        public static UnorderedListElement UnorderedList(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<UnorderedListElement>();
        }

        public static SpanElement Span(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<SpanElement>();
        }
    }
}