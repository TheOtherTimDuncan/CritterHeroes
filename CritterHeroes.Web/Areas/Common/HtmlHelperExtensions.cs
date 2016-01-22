using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Common.ActionExtensions;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using TOTD.Utility.Misc;

namespace CritterHeroes.Web.Areas.Common
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString CancelButton(this HtmlHelper htmlHelper)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.AddCssClass("btn btn-info");
            builder.SetInnerText("Cancel");
            builder.MergeAttribute("title", "Cancel");

            IPageContextService pageContextService = DependencyResolver.Current.GetService<IPageContextService>();
            PageContext pageContext = pageContextService.GetPageContext();

            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            string previousUrl = urlHelper.Local(pageContext.IfNotNull(x => x.PreviousPath));
            builder.MergeAttribute("href", previousUrl);

            return new MvcHtmlString(builder.ToString());
        }

        public static string ModelStateClassFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    return HtmlHelper.ValidationInputCssClassName;
                }
            }

            return null;
        }

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