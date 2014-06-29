using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Contracts;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class ListItemElement : BaseContainerElement<ListItemElement>
    {
        public ListItemElement(ViewContext viewContext)
            : base("li", viewContext)
        {
        }

        public ListItemElement Text(string text)
        {
            AddInnerHtml(text);
            return this;
        }
    }
}