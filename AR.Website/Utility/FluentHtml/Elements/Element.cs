using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Contracts;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class Element<T> : IElement where T : Element<T>, IElement
    {
        protected StringBuilder innerHtmlBuilder;

        public Element(string tag, ViewContext viewContext)
            : this(tag, TagRenderMode.Normal, viewContext)
        {
        }

        public Element(string tag, TagRenderMode renderMode, ViewContext viewContext)
        {
            this.Tag = tag;
            this.Builder = new TagBuilder(tag);
            this.UrlHelper = new UrlHelper(viewContext.RequestContext);
            this.TagRenderMode = renderMode;

            innerHtmlBuilder = new StringBuilder();
        }

        protected TagBuilder Builder
        {
            get;
            private set;
        }

        protected UrlHelper UrlHelper
        {
            get;
            set;
        }

        protected TagRenderMode TagRenderMode
        {
            get;
            set;
        }

        protected string Tag
        {
            get;
            private set;
        }

        public T Attribute(string name, object value)
        {
            var valueString = value == null ? null : value.ToString();
            Builder.MergeAttribute(name, valueString, true);
            return (T)this;
        }

        public T Class(string className)
        {
            Builder.AddCssClass(className);
            return (T)this;
        }

        public T Data(string name, object value)
        {
            var valueString = value == null ? null : value.ToString();
            Builder.MergeAttribute("data-" + name, valueString);
            return (T)this;
        }

        public string ToHtmlString()
        {
            Builder.InnerHtml = innerHtmlBuilder.ToString();
            string result = Builder.ToString(TagRenderMode);
            return result;
        }

        protected void AddInnerHtml(string text)
        {
            string encoded = HttpUtility.HtmlEncode(text);
            innerHtmlBuilder.Append(encoded);
        }

        protected void AddInnerHtml(IHtmlString innerHtml)
        {
            innerHtmlBuilder.Append(innerHtml.ToHtmlString());
        }
    }
}