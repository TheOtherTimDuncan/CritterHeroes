using System;

namespace CritterHeroes.Web.Models.Logging
{
    public enum UserActions
    {
        PasswordLoginSuccess,
        PasswordLoginFailure,
        DuplicateUsernameCheck,
        UsernameChanged,
        ForgotPasswordFailure,
        ForgotPasswordSuccess,
        ResetPasswordFailure,
        ResetPasswordSuccess
    }
}
