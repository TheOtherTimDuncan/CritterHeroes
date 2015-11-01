using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Common.Commands
{
    public class ResetPasswordEmailCommand : BaseTokenEmailCommand
    {
        public ResetPasswordEmailCommand(string emailTo)
            : base(EmailCommand.GetEmailNameFromCommandName(nameof(ResetPasswordEmailCommand)), emailTo)
        {
        }

        public string Url
        {
            get;
            set;
        }
    }
}
