using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Models;

namespace CritterHeroes.Web.Common.Commands
{
    public class FosterCrittersEmailCommand : EmailCommand<FosterCrittersEmailCommand.FosterCrittersEmailData>
    {
        public FosterCrittersEmailCommand(string emailTo)
            : base(nameof(FosterCrittersEmailCommand), emailTo)
        {
        }

        public class FosterCrittersEmailData : BaseEmailData
        {
            public IEnumerable<Object> Critters
            {
                get;
                set;
            }
        }
    }
}
