using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Shared.Commands
{
    public class ResetPasswordAttemptEmailCommand : EmailCommandBase
    {
        public ResetPasswordAttemptEmailCommand(string emailTo)
            : base(emailTo)
        {
        }
    }
}
