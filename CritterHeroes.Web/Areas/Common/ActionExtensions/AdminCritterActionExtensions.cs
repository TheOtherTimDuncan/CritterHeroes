using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Critters;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class AdminCritterActionExtensions
    {
        public static LinkElement AdminCrittersActionLink(this LinkElement linkElement, Expression<Func<CrittersController, ActionResult>> actionSelector)
        {
            return linkElement.ActionLink<CrittersController>(actionSelector);
        }
    }
}
