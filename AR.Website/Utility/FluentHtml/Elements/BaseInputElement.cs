using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using AR.Website.Utility.FluentHtml.Html;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class BaseInputElement<T> : Element<T> where T : BaseInputElement<T>
    {
        private HtmlHelper _htmlHelper;

        public BaseInputElement(string inputType, HtmlHelper htmlHelper)
            : base(HtmlTag.Input, TagRenderMode.SelfClosing, htmlHelper)
        {
            this._htmlHelper = htmlHelper;
            Builder.MergeAttribute(HtmlAttribute.Type, inputType);
        }

        public T For<TModel>(Expression<Func<TModel, object>> expression)
        {
            this.Name(GetElementNameFromExpression(expression));
            this.ID(GetElementIDFromExpression(expression));
            this.ValueFor<TModel>(expression);
            return (T)this;
        }

        public T Value(string value)
        {
            Builder.MergeAttribute(HtmlAttribute.Value, value);
            return (T)this;
        }

        public T ValueFor<TModel>(Expression<Func<TModel, object>> expression)
        {
            string value = GetElementValueFromExpression(expression);
            return Value(value);
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
    }
}