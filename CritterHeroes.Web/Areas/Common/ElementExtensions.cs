using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Account;
using CritterHeroes.Web.Areas.Home;
using TOTD.Mvc.Actions;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common
{
    public static class ElementExtensions
    {
        public static LinkElement HomeActionLink(this LinkElement linkElement, Expression<Func<HomeController, ActionResult>> actionSelector)
        {
            return linkElement.ActionLink<HomeController>(actionSelector);
        }

        public static FormElement LoginAction(this FormElement formElement, string returnUrl)
        {
            ActionHelperResult actionResult = ActionHelper.GetRouteValues<AccountController>(x => x.Login(null));
            return formElement.Action(actionResult.ActionName, actionResult.ControllerName, new
            {
                returnUrl = returnUrl
            });
        }

        public static LinkElement EditProfileLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink<AccountController>(x => x.EditProfile((string)null));
        }
    }
}