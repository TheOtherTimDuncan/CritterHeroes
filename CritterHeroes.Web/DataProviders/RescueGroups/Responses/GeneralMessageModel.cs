using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Responses
{
    public class GeneralMessageModel
    {
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
