using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AR.Website.Utility.FluentHtml.Bootstrap;
using AR.Website.Utility.FluentHtml.Conventions;

namespace AR.Website
{
    public static class DefaultHtmlConventions
    {
        public static void AddHtmlConventions()
        {
            new HtmlConventionBuilder().AddConvention<BootstrapNavItem>(x => x.SetActiveByControllerAndAction());
        }
    }
}