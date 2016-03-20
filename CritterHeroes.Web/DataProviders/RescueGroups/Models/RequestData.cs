using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class RequestData
    {
        public RequestData(string key, object data)
        {
            this.Key = key;
            this.Data = data;
        }

        public string Key
        {
            get;
        }

        public object Data
        {
            get;
        }
    }
}
