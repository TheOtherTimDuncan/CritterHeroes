using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Account;
using TOTD.Mvc.Actions;
using TOTD.Mvc.FluentHtml.Elements;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class AccountActionExtensions
    {
        public static string AccountLoginAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(AccountController.Login), ControllerRouteName);
        }

        public static string AccountEditProfileLoginAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(AccountController.EditProfileLogin), ControllerRouteName);
        }

        public static string AccountEditProfileSecureAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(AccountController.EditProfileSecure), ControllerRouteName);
        }

        public static string AccountConfirmEmailAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(AccountController.ConfirmEmail), ControllerRouteName);
        }

        public static string AccountForgotPasswordAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(AccountController.ForgotPassword), ControllerRouteName);
        }

        public static LinkElement AccountLoginLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink(nameof(AccountController.Login), ControllerRouteName);
        }

        public static LinkElement AccountLogoutLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink(nameof(AccountController.LogOut), ControllerRouteName);
        }

        public static LinkElement AccountEditProfileLink(this LinkElement linkElement)
        {
            return linkElement.ActionLink(nameof(AccountController.EditProfile), ControllerRouteName);
        }

        public static FormElement LoginAction(this FormElement formElement, string returnUrl)
        {
            return formElement.Action(nameof(AccountController.Login), ControllerRouteName, new
            {
                returnUrl = returnUrl
            });
        }

        private static string ControllerRouteName
        {
            get;
        } = ActionHelper.GetControllerRouteName(nameof(AccountController));
    }
}
