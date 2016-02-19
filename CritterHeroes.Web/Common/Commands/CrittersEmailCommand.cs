using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Models.Emails;

namespace CritterHeroes.Web.Common.Commands
{
    public class CrittersEmailCommand : EmailCommand<CrittersEmailCommand.CrittersEmailData>
    {
        public CrittersEmailCommand(string emailTo)
            : base(nameof(CrittersEmailCommand), emailTo)
        {
        }

        public class CrittersEmailData : BaseEmailData
        {
            public IEnumerable<Object> Critters
            {
                get;
                set;
            }
        }
    }
}
