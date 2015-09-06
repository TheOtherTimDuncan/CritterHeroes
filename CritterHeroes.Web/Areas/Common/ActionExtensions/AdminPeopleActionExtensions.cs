using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.People;
using TOTD.Mvc.Actions;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class AdminPeopleActionExtensions
    {
        public static LinkElement AdminPeopleActionLink(this LinkElement linkElement, Expression<Func<PeopleController, ActionResult>> actionSelector)
        {
            return linkElement.ActionLink<PeopleController>(actionSelector);
        }

        public static string AdminPeopleImportAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action<PeopleController>(x => x.ImportPeople(null));
        }

        public static string AdminBusinessImportAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action<PeopleController>(x => x.ImportBusinesses(null));
        }
    }
}
