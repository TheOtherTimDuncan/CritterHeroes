using System;
using System.Collections.Generic;
using System.IO;
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
            this.ViewWriter = viewContext.Writer;

            this.CurrentArea = (viewContext.RouteData.DataTokens[RouteDataKeys.Area] as string) ?? string.Empty; // We need an empty string not null so it will match correctly later
            this.CurrentControllerName = viewContext.RouteData.GetRequiredString(RouteDataKeys.Controller);
            this.CurrentActionName = viewContext.RouteData.GetRequiredString(RouteDataKeys.Action);

            string test = UrlHelper.Action(CurrentActionName, CurrentControllerName, viewContext.RouteData);

            innerHtmlBuilder = new StringBuilder();
        }

        protected TagBuilder Builder
        {
            get;
            private set;
        }

        protected TextWriter ViewWriter
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

        public string CurrentArea
        {
            get;
            private set;
        }

        public string CurrentControllerName
        {
            get;
            private set;
        }

        public string CurrentActionName
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

        public T Data(string name, object value)
        {
            var valueString = value == null ? null : value.ToString();
            Builder.MergeAttribute("data-" + name, valueString);
            return (T)this;
        }

        public string ToHtmlString()
        {
            PreRender();
            Builder.InnerHtml = innerHtmlBuilder.ToString();
            string result = Builder.ToString(TagRenderMode);
            return result;
        }

        protected virtual void PreRender()
        {
        }

        protected string HtmlEncode(string text)
        {
            return HttpUtility.HtmlEncode(text);
        }

        protected void AddInnerHtml(string text)
        {
            string encoded = HtmlEncode(text);
            innerHtmlBuilder.Append(encoded);
        }

        protected void AddInnerHtml(IHtmlString innerHtml)
        {
            innerHtmlBuilder.Append(innerHtml.ToHtmlString());
        }
    }
}