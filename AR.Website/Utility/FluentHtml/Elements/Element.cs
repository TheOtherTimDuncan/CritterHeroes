using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Contracts;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class Element<T> : IElement where T : Element<T>, IElement
    {
        public Element(string tag)
            : this(tag, TagRenderMode.Normal)
        {
        }

        public Element(string tag, TagRenderMode renderMode)
        {
            this.Builder = new TagBuilder(tag);
        }

        protected TagBuilder Builder
        {
            get;
            private set;
        }

        protected TagRenderMode TagRenderMode
        {
            get;
            set;
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

        public string ToHtmlString()
        {
            string result = Builder.ToString(TagRenderMode);
            return result;
        }
    }
}