using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Html;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class UnorderedListElement : BaseListElement<UnorderedListElement>
    {
        public UnorderedListElement(ViewContext viewContext)
            : base(HtmlTag.ListUnordered, viewContext)
        {
        }
    }
}