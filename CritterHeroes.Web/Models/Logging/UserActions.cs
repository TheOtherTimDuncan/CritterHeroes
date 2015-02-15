using System;

namespace CritterHeroes.Web.Models.Logging
{
    public enum UserActions
    {
        PasswordLoginSuccess,
        PasswordLoginFailure,
        DuplicateUsernameCheck,
        EmailChanged,
        ForgotPasswordFailure,
        ForgotPasswordSuccess,
        ResetPasswordFailure,
        ResetPasswordSuccess,
        ForgotUsernameFailure,
        ForgotUsernameSuccess,
        ResendConfirmationCodeFailure,
        ResendConfirmationCodeSuccess,
        ConfirmEmailSuccess,
        ConfirmEmailFailure
    }
}
