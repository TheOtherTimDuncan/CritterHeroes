using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Contracts;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class BaseContainerElement<T> : Element<T> where T : BaseContainerElement<T>
    {
        public BaseContainerElement(string tag, ViewContext viewContext)
            : base(tag, viewContext)
        {
        }

        public T AddElement(IElement element)
        {
            AddInnerHtml(element);
            return (T)this;
        }

        public T AddElement(Func<IElement> elementBuilder)
        {
            IElement element = elementBuilder();
            AddElement(element);
            return (T)this;
        }
    }
}