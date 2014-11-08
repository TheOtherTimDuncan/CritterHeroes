using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CH.Website.Utility.FluentHtml.Html;

namespace CH.Website.Utility.FluentHtml.Elements
{
    public class SpanElement : BaseContainerElement<SpanElement>
    {
        public SpanElement(HtmlHelper htmlHelper)
            : base(HtmlTag.Span, htmlHelper)
        {
        }

        public SpanElement Text(string text)
        {
            AddInnerHtml(text);
            return this;
        }
    }
}