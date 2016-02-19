using System;
using System.Collections.Generic;
using CritterHeroes.Web.Models.Emails;

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
