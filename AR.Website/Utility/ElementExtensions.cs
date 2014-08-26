using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using AR.Website.Controllers;
using AR.Website.Utility.FluentHtml.Elements;

namespace AR.Website.Utility
{
    public static class ElementExtensions
    {
        public static LinkElement HomeActionLink(this LinkElement linkElement, Expression<Func<HomeController, ActionResult>> actionSelector)
        {
            return linkElement.ActionLink<HomeController>(actionSelector);
        }
    }
}