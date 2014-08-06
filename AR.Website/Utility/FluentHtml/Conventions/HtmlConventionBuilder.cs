using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AR.Website.Utility.FluentHtml.Contracts;

namespace AR.Website.Utility.FluentHtml.Conventions
{
    public class HtmlConventionBuilder
    {
        public HtmlConventionBuilder AddConvention<T>(Action<T> action) where T : IElement
        {
            IHtmlConvention convention = new HtmlConvention<T>(action);
            ElementFactory.AddConvention(convention);
            return this;
        }
    }
}