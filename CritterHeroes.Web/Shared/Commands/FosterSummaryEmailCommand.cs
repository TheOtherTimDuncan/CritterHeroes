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
            public IEnumerable<Summary> Critters
            {
                get;
                set;
            }
        }

        public class Summary
        {
            public string FosterName
            {
                get;
                set;
            }

            public int BabyCount
            {
                get;
                set;
            }

            public int YoungCount
            {
                get;
                set;
            }

            public int AdultCount
            {
                get;
                set;
            }

            public int SeniorCount
            {
                get;
                set;
            }
        }
    }
}
