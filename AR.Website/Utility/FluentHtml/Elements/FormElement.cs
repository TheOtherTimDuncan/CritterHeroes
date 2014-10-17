using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Contracts;
using AR.Website.Utility.FluentHtml.Html;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class FormElement : BaseContainerElement<FormElement>
    {
        public FormElement(HtmlHelper htmlHelper)
            : base(HtmlTag.Form, htmlHelper)
        {
            Method(FormMethod.Post);  // Default
        }

        public FormElement Method(FormMethod formMethod)
        {
            Builder.MergeAttribute("method", HtmlHelper.GetFormMethodString(formMethod), replaceExisting: true);
            return this;
        }

        public FormElement AutoCompleteOff()
        {
            Builder.MergeAttribute(HtmlAttribute.AutoComplete, "off");
            return this;
        }
    }
}