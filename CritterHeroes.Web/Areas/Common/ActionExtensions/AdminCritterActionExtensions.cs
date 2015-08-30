using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Critters;
using TOTD.Mvc.Actions;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class AdminCritterActionExtensions
    {
        public static LinkElement AdminCrittersActionLink(this LinkElement linkElement, Expression<Func<CrittersController, Task<ActionResult>>> actionSelector)
        {
            return linkElement.ActionLink<CrittersController>(actionSelector);
        }

        public static string AdminCrittersUploadJsonAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action<CrittersController>(x => x.UploadJson(null));
        }
    }
}
