using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Features.Account;

namespace CritterHeroes.Web.Features.Shared.ActionExtensions
{
    public static class AccountActionExtensions
    {
        public static string AccountLoginAction(this UrlHelper urlHelper, string returnUrl = null)
        {
            return urlHelper.Action(nameof(AccountController.Login), AccountController.Route, new
            {
                returnUrl = returnUrl
            });
        }

        public static string AccountEditProfileLoginAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(AccountController.EditProfileLogin), AccountController.Route);
        }

        public static string AccountEditProfileSecureAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(AccountController.EditProfileSecure), AccountController.Route);
        }

        public static string AccountConfirmEmailAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(AccountController.ConfirmEmail), AccountController.Route);
        }

        public static string AccountForgotPasswordAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(AccountController.ForgotPassword), AccountController.Route);
        }

        public static string AccountLogoutAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(AccountController.LogOut), AccountController.Route);
        }

        public static string AccountEditProfileAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(AccountController.EditProfile), AccountController.Route);
        }

        public static string AccountResetPasswordAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(nameof(AccountController.ResetPassword), AccountController.Route);
        }

        public static string GenerateResetPasswordAbsoluteUrl(this IUrlGenerator urlGenerator, string token)
        {
            return urlGenerator.GenerateAbsoluteUrl(nameof(AccountController.ResetPassword), AccountController.Route, new
            {
                code = token
            });
        }

        public static string GenerateConfirmEmailAbsoluteUrl(this IUrlGenerator urlGenerator, string email, string confirmationCode)
        {
            return urlGenerator.GenerateAbsoluteUrl(nameof(AccountController.ConfirmEmail), AccountController.Route, new
            {
                email = email,
                confirmationCode = confirmationCode
            });
        }
    }
}
