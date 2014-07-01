using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Bootstrap;

namespace AR.Website.Utility.FluentHtml
{
    public static class BootstrapExtensions
    {
        public static BootstrapNav BootstrapNav(this HtmlHelper htmlHelper)
        {
            return new BootstrapNav(htmlHelper.ViewContext);
        }

        public static BootstrapNavItem BootstrapNavItem(this HtmlHelper htmlHelper)
        {
            return new Bootstrap.BootstrapNavItem(htmlHelper.ViewContext);
        }
    }
}