using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Domain.Contracts;
using CritterHeroes.Web.Features.Admin.Contacts;
using CritterHeroes.Web.Features.Admin.Contacts.Queries;

namespace CritterHeroes.Web.Features.Shared.ActionExtensions
{
    public static class AdminContactsActionExtensions
    {
        public static string GeneratePersonEditUrl(this IUrlGenerator urlGenerator, int personID)
        {
            return urlGenerator.GenerateSiteUrl(nameof(ContactsController.Person), ContactsController.Route, new PersonEditQuery()
            {
                PersonID = personID
            });
        }

        public static string GenerateBusinessEditUrl(this IUrlGenerator urlGenerator, int businessID)
        {
            return urlGenerator.GenerateSiteUrl(nameof(ContactsController.Business), ContactsController.Route, new BusinessEditQuery()
            {
                BusinessID = businessID
            });
        }

        public static string AdminContactsHomeAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(ContactsController.Index), ContactsController.Route, AreaName.AdminRouteValue);
        }

        public static string AdminContactsListAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(ContactsController.List), ContactsController.Route, AreaName.AdminRouteValue);
        }
    }
}
