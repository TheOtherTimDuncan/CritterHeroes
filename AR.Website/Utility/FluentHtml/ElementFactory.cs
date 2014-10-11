using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Contracts;

namespace AR.Website.Utility.FluentHtml
{
    public class ElementFactory
    {
        private static List<IHtmlConvention> _conventions = new List<IHtmlConvention>();

        private HtmlHelper _htmlHelper;

        public ElementFactory(HtmlHelper htmlHelper)
        {
            this._htmlHelper = htmlHelper;
        }

        public T CreateElement<T>() where T : IElement
        {
            T result = (T)Activator.CreateInstance(typeof(T), _htmlHelper.ViewContext);

            foreach (IHtmlConvention convention in _conventions)
            {
                convention.ApplyConvention(result);
            }

            return result;
        }

        public static void AddConvention(IHtmlConvention convention)
        {
            _conventions.Add(convention);
        }
    }

    public static class ElementFactoryExtension
    {
        public static ElementFactory FluentHtml(this HtmlHelper htmlHelper)
        {
            return new ElementFactory(htmlHelper);
        }
    }
}