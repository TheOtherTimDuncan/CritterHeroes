using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Html;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class InputTextElement : BaseInputElement<InputTextElement>
    {
        public InputTextElement(HtmlHelper htmlHelper)
            : base(HtmlInputType.Text, htmlHelper)
        {
        }
    }
}