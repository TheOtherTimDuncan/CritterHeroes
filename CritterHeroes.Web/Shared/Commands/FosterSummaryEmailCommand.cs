using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Shared.Commands
{
    public class FosterSummaryEmailCommand : EmailCommandBase
    {
        public FosterSummaryEmailCommand(string emailTo)
            : base(emailTo)
        {
        }

        public IEnumerable<Summary> Summaries
        {
            get;
            set;
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
