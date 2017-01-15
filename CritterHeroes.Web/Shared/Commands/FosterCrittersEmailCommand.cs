using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Models.Emails;

namespace CritterHeroes.Web.Shared.Commands
{
    public class FosterCrittersEmailCommand : EmailCommand<FosterCrittersEmailCommand.FosterCrittersEmailData>
    {
        public FosterCrittersEmailCommand(string emailTo)
            : base(nameof(FosterCrittersEmailCommand), emailTo)
        {
        }

        public class FosterCrittersEmailData : BaseEmailData
        {
            public IEnumerable<Critter> Critters
            {
                get;
                set;
            }
        }

        public class Critter
        {
            public string ThumbnailUrl
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }

            public string Status
            {
                get;
                set;
            }

            public string Sex
            {
                get;
                set;
            }

            public string RescueID
            {
                get;
                set;
            }

            public int? RescueGroupsID
            {
                get;
                set;
            }
        }
    }
}
