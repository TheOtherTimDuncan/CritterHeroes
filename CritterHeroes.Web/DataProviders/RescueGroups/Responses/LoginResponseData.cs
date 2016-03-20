using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Responses
{
    public class LoginResponseData
    {
       public string Token
        {
            get;
            set;
        }

        public string TokenHash
        {
            get;
            set;
        }
    }
}
