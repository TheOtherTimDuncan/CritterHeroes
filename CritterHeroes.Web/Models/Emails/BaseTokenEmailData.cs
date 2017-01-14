using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CritterHeroes.Web.Models.Emails
{
    public class BaseTokenEmailData : BaseEmailData
    {
        public string Token
        {
            get;
            set;
        }

        [JsonIgnore]
        public TimeSpan TokenLifespan
        {
            get;
            set;
        }

        [JsonProperty("tokenLifespan")]
        public string TokenLifespanDisplay
        {
            get
            {
                return string.Format($"{TokenLifespan.TotalHours} hours");
            }
        }
    }
}
