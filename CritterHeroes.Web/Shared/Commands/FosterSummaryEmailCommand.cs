using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Models.Emails;

namespace CritterHeroes.Web.Shared.Commands
{
    public class FosterSummaryEmailCommand : EmailCommand<FosterSummaryEmailCommand.FosterSummaryEmailData>
    {
        public FosterSummaryEmailCommand(string emailTo)
            : base(nameof(FosterSummaryEmailCommand), emailTo)
        {
        }

        public class FosterSummaryEmailData : BaseEmailData
        {
            public IEnumerable<Object> Critters
            {
                get;
                set;
            }
        }
    }
}
