using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Areas.Admin.Emails;
using TOTD.Mvc.Actions;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class EmailActionExtensions
    {
        public static LinkElement AdminEmailHomeActionLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink(nameof(EmailsController.Index), ControllerRouteName, AreaName.AdminRouteValue);
        }

        public static string ControllerRouteName
        {
            get;
        } = ActionHelper.GetControllerRouteName(nameof(EmailsController));
    }
}
