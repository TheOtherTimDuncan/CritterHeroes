using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Common.Commands
{
    public class ResetPasswordAttemptEmailCommand : BaseTokenEmailCommand
    {
        public ResetPasswordAttemptEmailCommand(string emailTo, string homeUrl)
            : base(EmailCommand.GetEmailNameFromCommandName(nameof(ResetPasswordAttemptEmailCommand)), emailTo)
        {
            this.HomeUrl = homeUrl;
        }

        public string HomeUrl
        {
            get;
            private set;
        }
    }
}
