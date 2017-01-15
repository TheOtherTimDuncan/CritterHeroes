using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Shared.Commands
{
    public class ConfirmEmailEmailCommand : EmailTokenCommandBase
    {
        public ConfirmEmailEmailCommand(string emailTo)
            : base(emailTo)
        {
        }

        public string UrlConfirm
        {
            get;
            set;
        }
    }
}
