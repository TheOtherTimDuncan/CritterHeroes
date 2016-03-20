using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Responses
{
    public class BaseResponseModel
    {
        public string Status
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public MessagesModel Messages
        {
            get;
            set;
        }
    }
}
