using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Lists;
using TOTD.Mvc.Actions;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class ListsActionExtensions
    {
        public static LinkElement ListsActionLink(this LinkElement linkElement, Expression<Func<ListsController, ActionResult>> actionSelector)
        {
            return linkElement.ActionLink<ListsController>(actionSelector);
        }

        public static string ListsRefreshAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action<ListsController>(x => x.Refresh(null));
        }

        public static string ListsSyncAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action<ListsController>(x => x.Sync(null));
        }
    }
}
