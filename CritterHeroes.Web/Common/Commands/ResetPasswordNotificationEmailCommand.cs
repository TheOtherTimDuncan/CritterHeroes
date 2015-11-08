using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Common.Commands
{
    public class ResetPasswordNotificationEmailCommand : EmailCommand<BaseEmailData>
    {
        public ResetPasswordNotificationEmailCommand(string emailTo)
            : base(nameof(ResetPasswordNotificationEmailCommand), emailTo)
        {
        }
    }
}
