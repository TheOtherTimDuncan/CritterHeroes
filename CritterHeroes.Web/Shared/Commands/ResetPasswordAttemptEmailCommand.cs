using System;
using System.Collections.Generic;
using CritterHeroes.Web.Models.Emails;

namespace CritterHeroes.Web.Shared.Commands
{
    public class ResetPasswordAttemptEmailCommand : EmailCommand<BaseEmailData>
    {
        public ResetPasswordAttemptEmailCommand(string emailTo)
            : base(nameof(ResetPasswordAttemptEmailCommand), emailTo)
        {
        }
    }
}
