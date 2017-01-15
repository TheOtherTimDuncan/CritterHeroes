using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Shared.Commands
{
    public class ResetPasswordEmailCommand : EmailTokenCommandBase
    {
        public ResetPasswordEmailCommand(string emailTo)
            : base(emailTo)
        {
        }

        public string UrlReset
        {
            get;
            set;
        }
    }
}
