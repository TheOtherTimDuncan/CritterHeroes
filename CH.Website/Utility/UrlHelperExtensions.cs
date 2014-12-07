using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using CH.Website.Controllers;
using TOTD.Mvc.Actions;

namespace CH.Website.Utility
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