using System;
using System.Collections.Generic;
using CritterHeroes.Web.Models.Emails;

namespace CritterHeroes.Web.Shared.Commands
{
    public class ConfirmEmailEmailCommand : EmailCommand<ConfirmEmailEmailCommand.ConfirmEmailData>
    {
        public ConfirmEmailEmailCommand(string emailTo)
            : base(nameof(ConfirmEmailEmailCommand), emailTo)
        {
        }

        public class ConfirmEmailData : BaseTokenEmailData
        {
            public string UrlConfirm
            {
                get;
                set;
            }
        }
    }
}
