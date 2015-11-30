﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Contacts;
using CritterHeroes.Web.Areas.Common.Models;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class AdminContactsActionExtensions
    {
        public static ControllerActionModel HomeAction(string title)
        {
            return new ControllerActionModel()
            {
                ControllerRoute = ContactsController.Route,
                ActionName = nameof(ContactsController.Index),
                AreaName = AreaName.Admin,
                Title = title
            };
        }

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
