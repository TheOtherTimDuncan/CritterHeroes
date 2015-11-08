using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Common.Commands
{
    public class ResetPasswordEmailCommand : EmailCommand<ResetPasswordEmailCommand.ResetPasswordEmailData>
    {
        public ResetPasswordEmailCommand(string emailTo)
            : base(nameof(ResetPasswordEmailCommand), emailTo)
        {
        }

        public class ResetPasswordEmailData : BaseTokenEmailData
        {
            public string UrlReset
            {
                get;
                set;
            }
        }
    }
}
