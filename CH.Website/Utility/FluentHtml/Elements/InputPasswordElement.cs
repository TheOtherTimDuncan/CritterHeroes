using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using CH.Website.Utility.FluentHtml.Html;

namespace CH.Website.Utility.FluentHtml.Elements
{
    public class InputPasswordElement : BaseInputElement<InputPasswordElement>
    {
        public InputPasswordElement(HtmlHelper htmlHelper)
            : base(HtmlInputType.Password, htmlHelper)
        {
        }
    }

    public class InputPasswordElement<TModel> : InputPasswordElement
    {
        public InputPasswordElement(HtmlHelper<TModel> htmlHelper)
            : base(htmlHelper)
        {
        }

        public InputPasswordElement For<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            SetAttributesFromModelProperty(expression);
            return this;
        }
    }
}
