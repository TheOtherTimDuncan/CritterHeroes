using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CritterHeroes.Web.Features.Shared
{
    public class ModelPropertyAttribute : IDisposable
    {
        public string ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public MvcHtmlString ValidationAttributes
        {
            get;
            set;
        }

        public MvcHtmlString ValidationMessage
        {
            get;
            set;
        }

        public void Dispose()
        {
        }
    }

    public static class HtmlPropertyHelper
    {
        public static ModelPropertyAttribute BeginProperty<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);

            ModelPropertyAttribute propertyAttribute = new ModelPropertyAttribute()
            {
                ID = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(expressionText),
                Name = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText),
                ValidationMessage = htmlHelper.ValidationMessage(expressionText)
            };

            StringBuilder sb = new StringBuilder();

            var attributes = htmlHelper.GetUnobtrusiveValidationAttributes(expressionText, metadata);
            foreach (var keyValue in attributes)
            {
                sb
                    .Append(keyValue.Key)
                    .Append("=\"")
                    .Append(htmlHelper.Encode(keyValue.Value))
                    .Append('"');
            }

            propertyAttribute.ValidationAttributes = MvcHtmlString.Create(sb.ToString());

            return propertyAttribute;
        }
    }
}
