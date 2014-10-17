using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using AR.Website.Utility.FluentHtml.Html;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class BaseFormElement<T>: Element<T> where T : BaseFormElement<T>
    {
        private HtmlHelper _htmlHelper;

        public BaseFormElement(string tag, TagRenderMode renderMode, HtmlHelper htmlHelper)
            : base(tag, renderMode, htmlHelper)
        {
            this._htmlHelper = htmlHelper;
        }

        public T AutoFocus()
        {
            Builder.MergeAttribute(HtmlAttribute.AutoFocus, HtmlAttribute.AutoFocus);
            return (T)this;
        }

        public T Required()
        {
            Builder.MergeAttribute(HtmlAttribute.Required, HtmlAttribute.Required);
            return (T)this;
        }

        public T ValueFor<TModel>(Expression<Func<TModel, object>> expression)
        {
            string value = GetElementValueFromExpression(expression);
            SetValue(value);
            return (T)this;
        }

        public T For<TModel>(Expression<Func<TModel, object>> expression)
        {
            string name = GetElementNameFromExpression(expression);

            this.Name(name);
            this.ID(GetElementIDFromExpression(expression));
            this.ValueFor<TModel>(expression);
            this.Builder.MergeAttributes(_htmlHelper.GetUnobtrusiveValidationAttributes(name));

            return (T)this;
        }

        protected string GetElementIDFromExpression(LambdaExpression expression)
        {
            string expressionName = ExpressionHelper.GetExpressionText(expression);
            return _htmlHelper.Id(expressionName).ToHtmlString();
        }

        protected string GetElementNameFromExpression(LambdaExpression expression)
        {
            string expressionName = ExpressionHelper.GetExpressionText(expression);
            return _htmlHelper.Name(expressionName).ToHtmlString();
        }

        protected string GetElementValueFromExpression(LambdaExpression expression)
        {
            string expressionName = ExpressionHelper.GetExpressionText(expression);
            MvcHtmlString htmlString = _htmlHelper.Value(expressionName);
            return htmlString.ToHtmlString();
        }

        protected virtual void SetValue(string value)
        {
        }
    }
}