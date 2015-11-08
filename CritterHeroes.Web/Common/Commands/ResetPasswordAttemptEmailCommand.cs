using System;
using System.Collections.Generic;

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
