using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Features.Admin.Contacts;

namespace CritterHeroes.Web.Features.Shared.ActionExtensions
{
    public static class AdminContactsActionExtensions
    {
        public static string AdminContactsHomeAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(ContactsController.Index), ContactsController.Route, AreaName.AdminRouteValue);
        }

        public static string AdminContactsListAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(ContactsController.List), ContactsController.Route, AreaName.AdminRouteValue);
        }

        public static string AdminPeopleImportAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(ContactsController.ImportPeople), ContactsController.Route, AreaName.AdminRouteValue);
        }

        public static string AdminBusinessImportAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(ContactsController.ImportBusinesses), ContactsController.Route, AreaName.AdminRouteValue);
        }
    }
}
