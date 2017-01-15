using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Shared.Commands
{
    public class ResetPasswordNotificationEmailCommand : EmailCommandBase
    {
        public ResetPasswordNotificationEmailCommand(string emailTo)
            : base(emailTo)
        {
        }
    }
}
