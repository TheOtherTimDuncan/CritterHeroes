using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Account;
using CritterHeroes.Web.Areas.Home;
using TOTD.Mvc.Actions;

namespace CritterHeroes.Web.Areas.Common
{
    public static class UrlHelperExtensions
    {
        public static string HomeAction(this UrlHelper urlHelper, Expression<Func<HomeController, ActionResult>> actionSelector)
        {
            return urlHelper.Action<HomeController>(actionSelector);
        }

        public static string AccountAction(this UrlHelper urlHelper, Expression<Func<AccountController, Task<ActionResult>>> actionSelector)
        {
            return urlHelper.Action<AccountController>(actionSelector);
        }

        public static string AccountAction(this UrlHelper urlHelper, Expression<Func<AccountController, ActionResult>> actionSelector)
        {
            return urlHelper.Action<AccountController>(actionSelector);
        }
    }
}