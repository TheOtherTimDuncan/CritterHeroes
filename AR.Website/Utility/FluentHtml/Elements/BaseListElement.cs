using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class BaseListElement<T> : BaseContainerElement<T> where T : BaseListElement<T>
    {
        public BaseListElement(string tag, ViewContext viewContext)
            : base(tag, viewContext)
        {
        }
    }
}