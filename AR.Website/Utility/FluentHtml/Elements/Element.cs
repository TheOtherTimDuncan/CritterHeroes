using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using AR.Website.Utility.FluentHtml.Contracts;
using AR.Website.Utility.FluentHtml.Html;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class Element<T> : IElement where T : Element<T>, IElement
    {
        private StringBuilder _innerHtmlBuilder;
        private HtmlHelper _htmlHelper;
        private ElementFactory _elementFactory;

        public Element(string tag, HtmlHelper htmlHelper)
            : this(tag, TagRenderMode.Normal, htmlHelper)
        {
        }

        public Element(string tag, TagRenderMode renderMode, HtmlHelper htmlHelper)
        {
            this.Tag = tag;
            this.Builder = new TagBuilder(tag);
            this.UrlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection);
            this.TagRenderMode = renderMode;
            this.ViewWriter = htmlHelper.ViewContext.Writer;

            this.CurrentArea = (htmlHelper.ViewContext.RouteData.DataTokens[RouteDataKeys.Area] as string) ?? string.Empty; // We need an empty string not null so it will match correctly later
            this.CurrentControllerName = htmlHelper.ViewContext.RouteData.GetRequiredString(RouteDataKeys.Controller);
            this.CurrentActionName = htmlHelper.ViewContext.RouteData.GetRequiredString(RouteDataKeys.Action);

            this._innerHtmlBuilder = new StringBuilder();
            this._elementFactory = new ElementFactory(htmlHelper);
            this._htmlHelper = htmlHelper;
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

        protected string ElementID
        {
            get;
            private set;
        }

        public string ElementName
        {
            get;
            set;
        }

        public T ID(string elementID)
        {
            this.ElementID = elementID;
            Builder.MergeAttribute(HtmlAttribute.ID, elementID);
            return (T)this;
        }

        public T Name(string elementName)
        {
            this.ElementName = elementName;
            Builder.MergeAttribute(HtmlAttribute.Name, elementName);
            return (T)this;
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
            Builder.InnerHtml = _innerHtmlBuilder.ToString();
            string result = Builder.ToString(TagRenderMode);
            return result;
        }

        protected ElementType CreateElement<ElementType>() where ElementType : IElement
        {
            return _elementFactory.CreateElement<ElementType>();
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
            _innerHtmlBuilder.Append(encoded);
        }

        protected void AddInnerHtml(IHtmlString innerHtml)
        {
            _innerHtmlBuilder.Append(innerHtml.ToHtmlString());
        }

        protected string GetElementIDFromExpression(LambdaExpression expression)
        {
            string expressionName = ExpressionHelper.GetExpressionText(expression);
            return _htmlHelper.Id(expressionName).ToHtmlString();
        }
    }
}