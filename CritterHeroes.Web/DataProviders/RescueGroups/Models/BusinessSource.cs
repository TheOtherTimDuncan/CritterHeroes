using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.DataProviders.RescueGroups.JsonConverters;
using Newtonsoft.Json;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class BusinessSource : BaseContactSource
    {
        [JsonProperty(PropertyName = "contactName")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contactCompany")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string Company
        {
            get;
            set;
        }
    }
}
