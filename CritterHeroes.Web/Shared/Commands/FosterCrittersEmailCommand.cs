using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Shared.Commands
{
    public class FosterCrittersEmailCommand : EmailCommandBase
    {
        public FosterCrittersEmailCommand(string emailTo)
            : base(emailTo)
        {
        }

        public IEnumerable<Critter> Critters
        {
            get;
            set;
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
