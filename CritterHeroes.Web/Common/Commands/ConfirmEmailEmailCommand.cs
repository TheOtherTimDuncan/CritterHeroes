using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CritterHeroes.Web.Common.Commands
{
    public class ConfirmEmailEmailCommand : BaseTokenEmailCommand
    {
        public ConfirmEmailEmailCommand(string emailTo)
            : base(EmailCommand.GetEmailNameFromCommandName(nameof(ConfirmEmailEmailCommand)), emailTo)
        {
        }

        public string HomeUrl
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }
    }
}
