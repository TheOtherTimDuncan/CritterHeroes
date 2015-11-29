using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Emails;
using CritterHeroes.Web.Areas.Common.Models;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class EmailActionExtensions
    {
        public static ControllerActionModel HomeAction(string title)
        {
            return new ControllerActionModel()
            {
                ControllerRoute = EmailsController.Route,
                ActionName = nameof(EmailsController.Index),
                AreaName = AreaName.Admin,
                Title = title
            };
        }
    }
}
