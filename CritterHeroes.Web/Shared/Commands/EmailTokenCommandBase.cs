using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Shared.Commands
{
    public abstract class EmailTokenCommandBase : EmailCommandBase
    {
        public EmailTokenCommandBase(string emailTo)
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
                return string.Format($"{TokenLifespan.TotalHours} hours");
            }
        }
    }
}
