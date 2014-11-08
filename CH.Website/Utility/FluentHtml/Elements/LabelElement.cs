using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using CH.Website.Utility.FluentHtml.Html;

namespace CH.Website.Utility.FluentHtml.Elements
{
    public class LabelElement :Element<LabelElement>
    {
        public LabelElement(HtmlHelper htmlHelper)
            : base(HtmlTag.Label, htmlHelper)
        {
        }

        public LabelElement Text(string text)
        {
            AddInnerHtml(text);
            return this;
        }

        public LabelElement For(string targetID)
        {
            Builder.MergeAttribute(HtmlAttribute.For, targetID);
            return this;
        }
    }

    public class LabelElement<TModel> : LabelElement
    {
        private HtmlHelper<TModel> _htmlHelper;

        public LabelElement(HtmlHelper<TModel> htmlHelper)
            : base(htmlHelper)
        {
            this._htmlHelper = htmlHelper;
        }

        public LabelElement For<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, _htmlHelper.ViewData);

            string targetID = ExpressionHelper.GetExpressionText(expression);
            this.For(TagBuilder.CreateSanitizedId(_htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(targetID)));

            string text = metadata.DisplayName ?? metadata.PropertyName ?? targetID.Split('.').Last();
            this.Text(text);

            return this;
        }
    }
}