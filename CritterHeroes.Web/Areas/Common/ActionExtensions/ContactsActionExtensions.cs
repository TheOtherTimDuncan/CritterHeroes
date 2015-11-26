using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Contacts;
using TOTD.Mvc.Actions;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class ContactsActionExtensions
    {
        public static LinkElement ContactsHomeActionLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink(nameof(ContactsController.Index), ControllerRouteName, AreaName.NoAreaRouteValue);
        }

        public static string ControllerRouteName
        {
            get;
        } = ActionHelper.GetControllerRouteName(nameof(ContactsController));
    }
}
