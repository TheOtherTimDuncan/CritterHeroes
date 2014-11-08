using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CH.Website.Utility.FluentHtml.Contracts;

namespace CH.Website.Utility.FluentHtml
{
    public class ElementFactory
    {
        private static List<IHtmlConvention> _conventions = new List<IHtmlConvention>();

        private HtmlHelper _htmlHelper;

        public ElementFactory(HtmlHelper htmlHelper)
        {
            this._htmlHelper = htmlHelper;
        }

        public virtual ElementType CreateElement<ElementType>() where ElementType : IElement
        {
            ElementType result = (ElementType)Activator.CreateInstance(typeof(ElementType), _htmlHelper);
            ApplyConventions(result);
            return result;
        }

        public static void AddConvention(IHtmlConvention convention)
        {
            _conventions.Add(convention);
        }

        protected void ApplyConventions(IElement element)
        {
            foreach (IHtmlConvention convention in _conventions)
            {
                convention.ApplyConvention(element);
            }
        }
    }

    public class ElementFactory<ModelType> : ElementFactory
    {
        private HtmlHelper<ModelType> _htmlHelper;

        public ElementFactory(HtmlHelper<ModelType> htmlHelper)
            : base(htmlHelper)
        {
            this._htmlHelper = htmlHelper;
        }

        public override ElementType CreateElement<ElementType>()
        {
            ElementType result = (ElementType)Activator.CreateInstance(typeof(ElementType), _htmlHelper);
            ApplyConventions(result);
            return result;
        }

    }

    public static class ElementFactoryExtension
    {
        public static ElementFactory FluentHtml(this HtmlHelper htmlHelper)
        {
            return new ElementFactory(htmlHelper);
        }

        public static ElementFactory<T> FluentHtml<T>(this HtmlHelper<T> htmlHelper)
        {
            return new ElementFactory<T>(htmlHelper);
        }
    }
}