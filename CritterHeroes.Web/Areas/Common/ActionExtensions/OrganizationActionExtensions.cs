using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Organizations;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class OrganizationActionExtensions
    {
        public static LinkElement OrganizationActionLink(this LinkElement linkElement, Expression<Func<OrganizationController, Task<ActionResult>>> actionSelector)
        {
            return linkElement.ActionLink<OrganizationController>(actionSelector);
        }
    }
}