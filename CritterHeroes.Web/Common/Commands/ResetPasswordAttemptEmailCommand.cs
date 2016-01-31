using System;
using System.Collections.Generic;
using CritterHeroes.Web.Models;

namespace CritterHeroes.Web.Common.Commands
{
    public class ResetPasswordAttemptEmailCommand : EmailCommand<BaseEmailData>
    {
        public ResetPasswordAttemptEmailCommand(string emailTo)
            : base(nameof(ResetPasswordAttemptEmailCommand), emailTo)
        {
        }
    }
}
