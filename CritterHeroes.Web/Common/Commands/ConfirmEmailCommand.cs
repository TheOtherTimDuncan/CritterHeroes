using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CritterHeroes.Web.Common.Commands
{
    public class ConfirmEmailCommand: BaseTokenEmailCommand
    {
        public ConfirmEmailCommand(string emailTo)
            : base(emailTo)
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