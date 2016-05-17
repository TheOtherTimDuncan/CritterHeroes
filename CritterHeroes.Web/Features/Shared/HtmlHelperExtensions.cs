using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CritterHeroes.Web.Features.Shared
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString ValidationAttributesFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            StringBuilder sb = new StringBuilder();

            var attributes = htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata);
            foreach (var keyValue in attributes)
            {
                sb
                    .Append(keyValue.Key)
                    .Append("=\"")
                    .Append(htmlHelper.Encode(keyValue.Value))
                    .Append('"');
            }

            return new MvcHtmlString(sb.ToString());
        }
    }
}
