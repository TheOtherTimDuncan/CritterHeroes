using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CH.Website.Utility.FluentHtml.Bootstrap;
using CH.Website.Utility.FluentHtml.Conventions;

namespace CH.Website
{
    public static class DefaultHtmlConventions
    {
        public static void AddHtmlConventions()
        {
            new HtmlConventionBuilder().AddConvention<BootstrapNavItem>(x => x.SetActiveByControllerAndAction());
        }
    }
}