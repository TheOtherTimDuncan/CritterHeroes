using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Features.Admin.Critters.Models
{
    public class StatusModel
    {
        public int StatusID
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public int Count
        {
            get;
            set;
        }

        public string CountDisplay
        {
            get
            {
                return Count.ToString("#,#");
            }
        }
    }
}
