using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Common.Commands
{
    public class ResetPasswordEmailCommand : BaseTokenEmailCommand
    {
        public ResetPasswordEmailCommand(string emailTo)
            : base(emailTo)
        {
        }

        public string Url
        {
            get;
            set;
        }
    }
}