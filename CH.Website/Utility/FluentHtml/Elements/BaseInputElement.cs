using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CH.Website.Utility.FluentHtml.Html;

namespace CH.Website.Utility.FluentHtml.Elements
{
    public class BaseInputElement<T> : BaseFormElement<T> where T : BaseInputElement<T>
    {
        public BaseInputElement(string inputType, HtmlHelper htmlHelper)
            : base(HtmlTag.Input, TagRenderMode.SelfClosing, htmlHelper)
        {
            Builder.MergeAttribute(HtmlAttribute.Type, inputType);
        }

        public T Placeholder(string text)
        {
            Builder.MergeAttribute(HtmlAttribute.Placeholder, text);
            return (T)this;
        }

        public T Value(string value)
        {
            Builder.MergeAttribute(HtmlAttribute.Value, value);
            return (T)this;
        }

        protected override void SetValue(string value)
        {
            base.SetValue(value);
            Value(value);
        }
    }
}