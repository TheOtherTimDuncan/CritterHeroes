using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Common.Models;
using CritterHeroes.Web.Areas.Contacts;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class ContactsActionExtensions
    {
        public static string ContactsHomeAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(ContactsController.Index), ContactsController.Route, AreaName.NoAreaRouteValue);
        }

        public static string ContactsListAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(ContactsController.List), ContactsController.Route, AreaName.NoAreaRouteValue);
        }

        public static ControllerActionModel HomeAction(string title)
        {
            return new ControllerActionModel()
            {
                ControllerRoute = ContactsController.Route,
                ActionName = nameof(ContactsController.Index),
                Title = title
            };
        }
    }
}
