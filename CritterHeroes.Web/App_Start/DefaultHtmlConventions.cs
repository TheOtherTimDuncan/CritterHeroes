using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TOTD.Mvc.FluentHtml.Bootstrap;
using TOTD.Mvc.FluentHtml.Conventions;

namespace CritterHeroes.Web
{
    public static class DefaultHtmlConventions
    {
        public static void AddHtmlConventions()
        {
            new HtmlConventionBuilder().AddConvention<BootstrapNavItem>(x => x.SetActiveByControllerAndAction());
        }
    }
}