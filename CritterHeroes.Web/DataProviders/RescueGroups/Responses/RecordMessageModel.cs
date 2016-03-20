using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Responses
{
    public class RecordMessageModel
    {
        public string Status
        {
            get;
            set;
        }

        public int ID
        {
            get;
            set;
        }

        public string MessageID
        {
            get;
            set;
        }

        public string MessageCriticality
        {
            get;
            set;
        }

        public string MessageText
        {
            get;
            set;
        }
    }
}
