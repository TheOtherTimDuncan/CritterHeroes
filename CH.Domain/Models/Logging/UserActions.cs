using System;

namespace CH.Domain.Models.Logging
{
    public enum UserActions
    {
        PasswordLoginSuccess,
        PasswordLoginFailure,
        DuplicateUsernameCheck,
    }
}
