using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Organizations;
using CritterHeroes.Web.Areas.Common.Models;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class OrganizationActionExtensions
    {
        public static ControllerActionModel EditProfileAction(string title)
        {
            return new ControllerActionModel()
            {
                ControllerRoute = OrganizationController.Route,
                ActionName = nameof(OrganizationController.EditProfile),
                AreaName = AreaName.Admin,
                Title = title
            };
        }
    }
}
