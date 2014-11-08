using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CH.Website.Utility.FluentHtml.Html;

namespace CH.Website.Utility.FluentHtml.Elements
{
    public class OrderedListElement : BaseListElement<OrderedListElement>
    {
        public OrderedListElement(HtmlHelper htmlHelper)
            : base(HtmlTag.ListOrdered, htmlHelper)
        {
        }
    }
}