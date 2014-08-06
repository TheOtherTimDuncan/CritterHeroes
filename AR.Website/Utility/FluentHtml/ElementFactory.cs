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

        public static T CreateElement<T>(ViewContext viewContext) where T : IElement
        {
            T result = (T)Activator.CreateInstance(typeof(T), viewContext);

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
}