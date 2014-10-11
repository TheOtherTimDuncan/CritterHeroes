using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Html;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class BaseListItemElement<T> : BaseContainerElement<T> where T : BaseListItemElement<T>
    {
        public BaseListItemElement(ViewContext viewContext)
            : base(HtmlTag.ListItem, viewContext)
        {
        }

        public T Text(string text)
        {
            AddInnerHtml(text);
            return (T)this;
        }
    }
}