using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Common.Commands
{
    public class BaseTokenEmailCommand : EmailCommand
    {
        public BaseTokenEmailCommand(string emailTo)
            : base(emailTo)
        {
        }

        public string Token
        {
            get;
            set;
        }

        public TimeSpan TokenLifespan
        {
            get;
            set;
        }

        public string TokenLifespanDisplay
        {
            get
            {
                return string.Format("{0} hours", TokenLifespan.TotalHours);
            }
        }
    }
}