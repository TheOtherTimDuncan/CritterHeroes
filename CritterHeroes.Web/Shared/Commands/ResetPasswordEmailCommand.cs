using System;
using System.Collections.Generic;
using CritterHeroes.Web.Models.Emails;

namespace CritterHeroes.Web.Shared.Commands
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
