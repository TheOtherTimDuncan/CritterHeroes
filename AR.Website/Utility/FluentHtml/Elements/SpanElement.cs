using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class SpanElement : BaseContainerElement<SpanElement>
    {
        public SpanElement(ViewContext viewContext)
            : base("span", viewContext)
        {
        }

        public SpanElement Text(string text)
        {
            AddInnerHtml(text);
            return this;
        }
    }
}