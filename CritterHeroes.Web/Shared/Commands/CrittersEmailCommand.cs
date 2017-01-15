using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Models.Emails;

namespace CritterHeroes.Web.Shared.Commands
{
    public class CrittersEmailCommand : EmailCommand<CrittersEmailCommand.CrittersEmailData>
    {
        public CrittersEmailCommand(string emailTo)
            : base(nameof(CrittersEmailCommand), emailTo)
        {
        }

        public class CrittersEmailData : BaseEmailData
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

            public string Location
            {
                get;
                set;
            }

            public string FosterName
            {
                get;
                set;
            }

            public string FosterEmail
            {
                get;
                set;
            }
        }
    }
}
