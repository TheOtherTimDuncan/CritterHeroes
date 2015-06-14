using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Account;
using TOTD.Mvc.Actions;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class AccountActionExtensions
    {
        public static string AccountAction(this UrlHelper urlHelper, Expression<Func<AccountController, Task<ActionResult>>> actionSelector)
        {
            return urlHelper.Action<AccountController>(actionSelector);
        }

        public static string AccountAction(this UrlHelper urlHelper, Expression<Func<AccountController, ActionResult>> actionSelector)
        {
            return urlHelper.Action<AccountController>(actionSelector);
        }

        public static FormElement LoginAction(this FormElement formElement, string returnUrl)
        {
            ActionHelperResult actionResult = ActionHelper.GetRouteValues<AccountController>(x => x.Login(null));
            return formElement.Action(actionResult.ActionName, actionResult.ControllerName, new
            {
                returnUrl = returnUrl
            });
        }

        public static LinkElement AccountActionLink(this LinkElement linkElement, Expression<Func<AccountController, Task<ActionResult>>> actionSelector)
        {
            return linkElement.ActionLink<AccountController>(actionSelector);
        }

        public static LinkElement AccountActionLink(this LinkElement linkElement, Expression<Func<AccountController, ActionResult>> actionSelector)
        {
            return linkElement.ActionLink<AccountController>(actionSelector);
        }
    }
}