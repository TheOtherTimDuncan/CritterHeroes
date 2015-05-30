using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Lists;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class ListsActionExtensions
    {
        public static LinkElement ListsActionLink(this LinkElement linkElement, Expression<Func<ListsController, ActionResult>> actionSelector)
        {
            return linkElement.ActionLink<ListsController>(actionSelector);
        }
    }
}