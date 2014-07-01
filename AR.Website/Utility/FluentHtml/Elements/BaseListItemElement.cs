using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class BaseListItemElement<T> : BaseContainerElement<T> where T : BaseListItemElement<T>
    {
        public BaseListItemElement(ViewContext viewContext)
            : base("li", viewContext)
        {
        }

        public T Text(string text)
        {
            AddInnerHtml(text);
            return (T)this;
        }
    }
}