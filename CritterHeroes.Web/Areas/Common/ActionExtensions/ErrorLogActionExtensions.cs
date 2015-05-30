using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.ErrorLog;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class ErrorLogActionExtensions
    {
        public static LinkElement ErrorLogActionLink(this LinkElement linkElement, Expression<Func<ErrorLogController, ActionResult>> actionSelector)
        {
            return linkElement.ActionLink<ErrorLogController>(actionSelector);
        }
    }
}