using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Common.Commands
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
