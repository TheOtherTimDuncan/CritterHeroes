using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Html;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class InputRadioElement : BaseInputElement<InputRadioElement>
    {
        public InputRadioElement(HtmlHelper htmlHelper)
            : base(HtmlInputType.Radio, htmlHelper)
        {
        }
    }
}